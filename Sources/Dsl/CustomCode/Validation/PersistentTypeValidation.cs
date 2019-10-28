using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Validation;
using System.Linq;
using TXSoftware.DataObjectsNetEntityModel.Common;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    public static class PersistentTypeValidation
    {
        private const string ERROR_MISSING_NAME = "Name must be specified for type '{0}'";
        private const string WARNING_NO_PROPETIES = "Structure '{0}' has no scalar or navigation properties assigned, an empty structure class will be generated.";
        private const string ERROR_INVALID_IS_INHERITED_PROPERTY = "{0} '{1}' has {2} property '{3}' marked as 'Is Inherited Property' but no inheritance path contains such property!";
        
        private const string CODE_MISSING_NAME = "MissingName";
        private const string CODE_PROPERTIES = "Properties";
        private const string CODE_INVALID_IS_INHERITED_PROPERTY = "InvalidIsInheritedProperty";

        internal static void ValidateName<T>(T persistentType, ValidationContext context) where T: PersistentType
        {
            if (string.IsNullOrEmpty(persistentType.Name))
            {
                context.LogError(string.Format(ERROR_MISSING_NAME, persistentType.Name),
                    CODE_MISSING_NAME, new ModelElement[] { persistentType });
            }
        }

        internal static void ValidateStructureProperties(Structure persistentType, ValidationContext context)
        {
            if (persistentType.Properties.Count == 0 && persistentType.NavigationProperties.Count == 0)
            {
                context.LogWarning(string.Format(WARNING_NO_PROPETIES, persistentType.Name), CODE_PROPERTIES,
                                   new ModelElement[] { persistentType });
            }
        }

        internal static void ValidateProperties<T>(T persistentType, InheritanceTree inheritanceTree, ValidationContext context) where T : PersistentType
        {
            if (persistentType is IInterface)
            {
                // validate 'IsInheritedProperty' properties, each property marked with this must have property with same name in its inheritance tree path
                IEnumerable<IPropertyBase> properties = persistentType.AllProperties.Where(property => property.IsInherited);

                if (properties.Count() > 0)
                {
                    IInterface @interface = (IInterface)persistentType;
                    if (inheritanceTree == null)
                    {
                        inheritanceTree = @interface.GetInheritanceTree();
                        inheritanceTree.RebuildTree(true);
                    }

                    foreach (var property in properties)
                    {
                        bool foundProperty = false;

                        inheritanceTree.IterateTree(false,
                                                    delegate(GenericTreeIterationArgs<InheritanceNode> args)
                                                    {
                                                        IInterface currentType = args.Current.Interface;
                                                        foundProperty = currentType != persistentType &&
                                                            currentType.AllProperties.Any(item => Util.StringEqual(item.Name, property.Name, true) &&
                                                            item.PropertyKind == property.PropertyKind);

                                                        args.Cancel = foundProperty;
                                                    });

                        if (!foundProperty)
                        {
                            context.LogError(
                                string.Format(
                                    ERROR_INVALID_IS_INHERITED_PROPERTY,
                                    persistentType.TypeKind, persistentType.Name, property.PropertyKind, property.Name),
                                CODE_INVALID_IS_INHERITED_PROPERTY,
                                new[] {property as ModelElement});
                        }
                    }
                }
            }

            ValidateOrmAttributes(persistentType, context);
        }

        private static void ValidateOrmAttributes<T>(T persistentType, ValidationContext context) where T : PersistentType
        {
            foreach (var typeAttribute in (persistentType as IPersistentType).TypeAttributes)
            {
                //OrmAttributesValidation.Validate(persistentType, typeAttribute, context);
                typeAttribute.Validate(context, persistentType);
            }

            foreach (var propertyBase in persistentType.AllProperties)
            {
                foreach (var typeAttribute in propertyBase.TypeAttributes)
                {
                    //OrmAttributesValidation.Validate(persistentType, typeAttribute, context);
                    typeAttribute.Validate(context, (ModelElement)propertyBase);
                }
            }
        }

        internal static DuplicatedPropertiesInfo FindDuplicatedPropertieInInheritanceTree(IInterface thisInterface, InheritanceTree inheritanceTree)
        {
            if (inheritanceTree == null)
            {
                inheritanceTree = thisInterface.GetInheritanceTree();
                inheritanceTree.RebuildTree(true);
            }
            
            var inheritanceTreeList = inheritanceTree.GetFlatList(InheritanceListMode.WholeTree).Select(node => node.Interface);

            List<IPropertyBase> scalarPropertiesWithDifferentType = new List<IPropertyBase>();
            List<IPropertyBase> scalarPropertiesWithSameType = new List<IPropertyBase>();

            List<IPropertyBase> structurePropertiesWithDifferentType = new List<IPropertyBase>();
            List<IPropertyBase> structurePropertiesWithSameType = new List<IPropertyBase>();

            List<IPropertyBase> navigationPropertiesWithDifferentType = new List<IPropertyBase>();
            List<IPropertyBase> navigationPropertiesWithSameType = new List<IPropertyBase>();

            // iterate scalar properties
            foreach (var scalarProperty in thisInterface.GetScalarProperties())
            {
                if (scalarProperty.IsInherited)
                {
                    continue;
                }

                var query = from childInterface in inheritanceTreeList
                            let childScalarProperties =
                                childInterface.Properties.Where(item => item.PropertyKind == PropertyKind.Scalar).Cast
                                <IScalarProperty>()
                            where childScalarProperties.Any(childScalarProperty
                                                            =>
                                                            Util.StringEqual(childScalarProperty.Name, scalarProperty.Name, true))
                            select new
                                   {
                                       PropertiesWithDifferentType = childScalarProperties
                                            .Where(childScalarProperty => !childScalarProperty.Type.EqualsTo(scalarProperty.Type)),
                                       PropertiesWithSameType = childScalarProperties
                                       .Where(childScalarProperty => childScalarProperty.Type.EqualsTo(scalarProperty.Type))
                                   };


                scalarPropertiesWithDifferentType.AddRange(
                    query.SelectMany(arg => arg.PropertiesWithDifferentType).Cast<IPropertyBase>().ToList());

                scalarPropertiesWithSameType.AddRange(
                    query.SelectMany(arg => arg.PropertiesWithSameType).Cast<IPropertyBase>().ToList());
            }


            // iterate structures properties
            foreach (var structureProperty in thisInterface.GetStructureProperties())
            {
                if (structureProperty.IsInherited)
                {
                    continue;
                }

                var query = from childInterface in inheritanceTreeList
                            let childStructureProperties =
                                childInterface.Properties.Where(item => item.PropertyKind == PropertyKind.Structure).Cast
                                <IStructureProperty>()
                            where childStructureProperties.Any(childStructureProperty
                                                            =>
                                                            Util.StringEqual(childStructureProperty.Name, structureProperty.Name, true))
                            select new
                            {
                                PropertiesWithDifferentType = childStructureProperties
                                     .Where(childScalarProperty => childScalarProperty.TypeOf != structureProperty.TypeOf),
                                PropertiesWithSameType = childStructureProperties
                                .Where(childScalarProperty => childScalarProperty.TypeOf == structureProperty.TypeOf)
                            };

                structurePropertiesWithDifferentType.AddRange(
                    query.SelectMany(arg => arg.PropertiesWithDifferentType).Cast<IPropertyBase>().ToList());

                structurePropertiesWithSameType.AddRange(
                    query.SelectMany(arg => arg.PropertiesWithSameType).Cast<IPropertyBase>().ToList());
            }

            // iterate navigation properties
            foreach (var navigationProperty in thisInterface.NavigationProperties)
            {
                if (navigationProperty.IsInherited)
                {
                    continue;
                }

                var query = from childInterface in inheritanceTreeList
                            let childNavigationProperties = childInterface.NavigationProperties
                            where childNavigationProperties.Any(childStructureProperty
                                                            =>
                                                            Util.StringEqual(childStructureProperty.Name, navigationProperty.Name, true))
                            select new
                            {
                                PropertiesWithDifferentType = childNavigationProperties
                                     .Where(childScalarProperty => !childScalarProperty.PersistentTypeHasAssociations.EqualAssociationLinkTo(navigationProperty.PersistentTypeHasAssociations)),
                                PropertiesWithSameType = childNavigationProperties
                                .Where(childScalarProperty => childScalarProperty.PersistentTypeHasAssociations.EqualAssociationLinkTo(navigationProperty.PersistentTypeHasAssociations))
                            };

                navigationPropertiesWithDifferentType.AddRange(
                    query.SelectMany(arg => arg.PropertiesWithDifferentType).Cast<IPropertyBase>().ToList());

                navigationPropertiesWithSameType.AddRange(
                    query.SelectMany(arg => arg.PropertiesWithSameType).Cast<IPropertyBase>().ToList());
            }

            var propertiesWithDifferentType =
                scalarPropertiesWithDifferentType.Concat(structurePropertiesWithDifferentType).Concat(
                    navigationPropertiesWithDifferentType);
            
            var propertiesWithSameType =
                scalarPropertiesWithSameType.Concat(structurePropertiesWithSameType).Concat(
                    navigationPropertiesWithSameType);

            int inheritancePathsCount = inheritanceTree.GetUniquePathsSuperRoots().Count();

            DuplicatedPropertiesInfo info = new DuplicatedPropertiesInfo(propertiesWithDifferentType, propertiesWithSameType,
                inheritancePathsCount);
            return info;
        }
    }

    #region class DuplicatedPropertiesInfo

    internal class DuplicatedPropertiesInfo
    {
        internal IEnumerable<IPropertyBase> PropertiesWithDifferentType { get; private set; }
        internal IEnumerable<IPropertyBase> PropertiesWithSameType { get; private set; }
        internal int InheritancePathsCount { get; private set; }

        public DuplicatedPropertiesInfo(IEnumerable<IPropertyBase> propertiesWithDifferentType,
            IEnumerable<IPropertyBase> propertiesWithSameType, int inheritancePathsCount)
        {
            PropertiesWithDifferentType = propertiesWithDifferentType;
            PropertiesWithSameType = propertiesWithSameType;
            this.InheritancePathsCount = inheritancePathsCount;
        }
    }

    #endregion class DuplicatedPropertiesInfo
}