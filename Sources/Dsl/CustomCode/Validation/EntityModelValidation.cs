using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Validation;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal static class EntityModelValidation
    {
        private const string ERROR_DUPLICATED_PROPERTY_SAME_TYPE = "{0} '{1}' contains property '{2}' which has duplicate definition(s) in: {3}";
        private const string ERROR_DUPLICATED_PROPERTY_DIFFERENT_TYPE = "{0} '{1}' contains property '{2}' which has different type in duplicate definition(s): {3}";

        private const string CODE_DUPLICATED_PROPERTY = "DuplicatedPropertyDefinition";
        private const string CODE_DUPLICATED_PROPERTY_DIFFERENT_TYPE = "DuplicatedPropertyDifferentTypeDefinition";
        private const string CODE_DUPLICATED_DIRECT_INHERITANCE = "DuplicatedDirectInheritance";

        internal static void ValidateInheritanceTree(EntityModel entityModel, ValidationContext context)
        {
            context.BeginValidationGlobalStage(ValidationGlobalStage.EntityModelInheritanceTree);

            try
            {
                // get types which are on top of hierarchy, means that these types are not referenced as interfaces implementation nor as base type
                // for any other types
                var topHierarchyTypes = entityModel.TopHierarchyTypes;

                var inheritanceTrees = new Dictionary<IInterface, InheritanceTree>();

                foreach (IInterface hierarchyType in topHierarchyTypes)
                {
                    InheritanceTree inheritanceTree = hierarchyType.GetInheritanceTree();
                    inheritanceTree.RebuildTree(true);

                    if (!inheritanceTrees.ContainsKey(hierarchyType))
                    {
                        inheritanceTrees.Add(hierarchyType, inheritanceTree);
                    }

                    var duplicatedDirectInheritances = inheritanceTree.FoundDuplicatedDirectInheritances();
                    foreach (var inheritanceDetail in duplicatedDirectInheritances)
                    {
                        string duplicatedItems = string.Join(";", inheritanceDetail.Items.Select(item => string.Format("{0}:{1}", item.Interface.Name,
                                                                                                                       item.Type == InheritanceType.Direct ? "D" : "I")));

                        string error =
                            string.Format(
                                "{0} '{1}' has direct inheritance to type '{2}', but {0} '{1}' is inherited(directly:D or indirectly:I) from other types ({3}) which already inheriting type '{2}'. Please remove direct inheritance '{2}' from type '{1}'",
                                hierarchyType.TypeKind, hierarchyType.Name, inheritanceDetail.Owner.Name, duplicatedItems);

                        var inheritanceLink = InterfaceInheritInterfaces.GetLink((Interface) hierarchyType, (Interface) inheritanceDetail.Owner);

                        context.LogError(error, CODE_DUPLICATED_DIRECT_INHERITANCE, new[] { inheritanceLink });
                    }
               
                }

                // continue only if revious check does not log any error
                if (!context.HasViolations())
                {
                    foreach (var hierarchyType in topHierarchyTypes)
                    {
                        InheritanceTree inheritanceTree = null;
                        if (inheritanceTrees.ContainsKey(hierarchyType))
                        {
                            inheritanceTree = inheritanceTrees[hierarchyType];
                        }

                        var duplicatedPropertiesInfo =
                            PersistentTypeValidation.FindDuplicatedPropertieInInheritanceTree(hierarchyType, inheritanceTree);

                        var duplicatedProperties = duplicatedPropertiesInfo.PropertiesWithSameType;
                        bool logAsWarning = hierarchyType.TypeKind == PersistentTypeKind.Interface;
                        IterateThroughDuplicatedProperties(hierarchyType, duplicatedProperties, false, logAsWarning, context);

                        duplicatedProperties = duplicatedPropertiesInfo.PropertiesWithDifferentType;
                        logAsWarning = false;
                        IterateThroughDuplicatedProperties(hierarchyType, duplicatedProperties, true, logAsWarning, context);

                        if (duplicatedPropertiesInfo.InheritancePathsCount > 1)
                        {
                            context.LogWarning(
                                string.Format(
                                    "{0} '{1}' has multiple({2}) inheritance paths, this may cause an inconsistent property types and/or attributes generated",
                                    hierarchyType.TypeKind, hierarchyType.Name,
                                    duplicatedPropertiesInfo.InheritancePathsCount),
                                "MultipleInheritancePaths", new ModelElement[] {hierarchyType as Interface});
                        }

                        if (context.CountViolations(ViolationType.Error, ViolationType.Fatal) > 0)
                        {
                            PersistentTypeValidation.ValidateProperties((PersistentType)hierarchyType, inheritanceTree, context);
                        }
                    }
                }
            }
            finally
            {
                context.EndValidationGlobalStage(ValidationGlobalStage.EntityModelInheritanceTree);
            }
        }

        private static void IterateThroughDuplicatedProperties(IInterface hierarchyType, 
            IEnumerable<IPropertyBase> duplicatedProperties, bool isDifferentTypes,
            bool logAsWarning, ValidationContext context) 
        {
            var query = from prop in duplicatedProperties
                        group prop by prop.Name
                        into g
                        select new
                               {
                                   Property = g.Key,
                                   Properties = from o in g select o,
                                   Owners = from o in g select o.Owner
                               };

            Func<IPropertyBase, string> getPropTypeName = property =>
            {
                switch (property.PropertyKind)
                {
                    case PropertyKind.Scalar:
                    {
                        return (property as IScalarProperty).Type.FullName;
                    }
                    case PropertyKind.Structure:
                    {
                        return (property as IStructureProperty).TypeOf == null
                                   ? string.Empty
                                   : (property as IStructureProperty).TypeOf.Name;
                    }
                    case PropertyKind.Navigation:
                    {
                        INavigationProperty navProp = (INavigationProperty) property;
                        var associations = navProp.PersistentTypeHasAssociations;
                        return string.Format("(Source->{0}, Target->{1})", 
                            associations.SourcePersistentType == null ? string.Empty : associations.SourcePersistentType.Name,
                            associations.TargetPersistentType == null ? string.Empty : associations.TargetPersistentType.Name);
                    }
                    default:
                        return string.Empty;
                }
            };

            foreach (var item in query)
            {
                var propertyName = item.Property;

                StringBuilder duplicatesIn = new StringBuilder();
                for (int i = 0; i < item.Owners.Count(); i++)
                {
                    IPropertyBase propertyItem = item.Properties.Skip(i).Take(1).SingleOrDefault();
                    bool propertyIsInherited = propertyItem.IsInherited;
                    IPersistentType propertyOwner = item.Owners.Skip(i).Take(1).Single();

                    if (isDifferentTypes)
                    {
                        duplicatesIn.AppendFormat("{0}->'{1}':{2},", propertyOwner.Name, propertyItem.Name, getPropTypeName(propertyItem));
                    }
                    else
                    {
                        duplicatesIn.AppendFormat("{0},", propertyOwner.Name);
                    }
                }

                var owners = item.Owners.Cast<PersistentType>().ToArray();

                string msg = isDifferentTypes
                                 ? ERROR_DUPLICATED_PROPERTY_DIFFERENT_TYPE
                                 : ERROR_DUPLICATED_PROPERTY_SAME_TYPE;
                string code = isDifferentTypes ? CODE_DUPLICATED_PROPERTY_DIFFERENT_TYPE : CODE_DUPLICATED_PROPERTY;

                string message = string.Format(msg, 
                    hierarchyType.TypeKind, hierarchyType.Name, propertyName, duplicatesIn);

                if (logAsWarning)
                {
                    context.LogWarning(message, code, owners);
                }
                else
                {
                    context.LogError(message, code, owners);
                }
            }
        }
    }
}