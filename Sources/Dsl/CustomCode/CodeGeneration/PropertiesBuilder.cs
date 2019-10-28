using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TXSoftware.DataObjectsNetEntityModel.Common;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    public sealed class PropertiesBuilderContext
    {
        private static PropertiesBuilderContext current;

        private readonly object sync = new object();
        private readonly Dictionary<IPersistentType, IPropertiesBuilder> propertiesBuilders = new Dictionary<
            IPersistentType, IPropertiesBuilder>();

        public static PropertiesBuilderContext Current
        {
            get
            {
                if (current == null)
                {
                    throw new ApplicationException("PropertiesBuilderContext is not initialized!");
                }

                return current;
            }
        }

        public static void Initialize(IModelRoot modelRoot)
        {
            current = new PropertiesBuilderContext();
            current.Build(modelRoot);
        }

        public IPropertiesBuilder Get(IPersistentType persistentType)
        {
            lock (sync)
            {
                return propertiesBuilders[persistentType];
            }
        }

        public bool Contains(IPersistentType persistentType)
        {
            lock (sync)
            {
                return propertiesBuilders.ContainsKey(persistentType);
            }
        }

        public bool IsInheritedProperty(IPersistentType persistentType, IPropertyBase property)
        {
            IPropertiesBuilder propertiesBuilder = Get(persistentType);
            return propertiesBuilder != null && propertiesBuilder.GetInheritedProperties().Contains(property);
        }

        private void Build(IModelRoot modelRoot)
        {
            var inheritanceTrees = InheritanceTreeCache.Get(modelRoot.TopHierarchyTypes.ToArray());
            foreach (var tree in inheritanceTrees)
            {
                if (!tree.TreeRebuilded)
                {
                    tree.RebuildTree(true);
                }
            }
            var mergePaths = InheritanceTree.MergePaths(inheritanceTrees);
            var dependencyImplementationFlatList = InheritanceTree.GetDependencyImplementationFlatList(mergePaths);
            foreach (var @interface in dependencyImplementationFlatList)
            {
                IPropertiesBuilder propertiesBuilder = PropertiesBuilder.Build(@interface);
                lock (sync)
                {
                    propertiesBuilders.Add(@interface, propertiesBuilder);
                }
            }

            // process rest types (e.g. with no inheritance defined)
            foreach (var @interface in modelRoot.PersistentTypes.Except(dependencyImplementationFlatList))
            {
                IPropertiesBuilder propertiesBuilder = PropertiesBuilder.Build(@interface);
                lock (sync)
                {
                    propertiesBuilders.Add(@interface, propertiesBuilder);
                }
            }
        }
    }

    internal sealed class PropertiesBuilder : IPropertiesBuilder
    {
        private readonly IPersistentType persistentType;
        private readonly List<IOrmAttribute> mergedTypeAttributes = new List<IOrmAttribute>();
        private readonly Dictionary<IPropertyBase, IPropertyBase> mappingProperties = new Dictionary<IPropertyBase, IPropertyBase>();

        private readonly Dictionary<IPropertyBase, IOrmAttribute[]> mappingPropertiesTypeAttributes =
            new Dictionary<IPropertyBase, IOrmAttribute[]>();

        private readonly List<IPropertyBase> mappingInheritedProperties = new List<IPropertyBase>();
        private InheritanceTree inheritanceTree;
        private ReadOnlyCollection<InheritanceNode> allInheritanceTypes;
        private IInterface thisInterface;

        private PropertiesBuilder(IPersistentType persistentType)
        {
            if (persistentType == null)
            {
                throw new ArgumentNullException("persistentType");
            }

            this.persistentType = persistentType;
            
            foreach (var typeAttribute in persistentType.TypeAttributes)
            {
                mergedTypeAttributes.Add(typeAttribute);
            }

            foreach (var property in persistentType.AllProperties)
            {
                mappingProperties.Add(property, property);
            }
        }

        public IEnumerable<IOrmAttribute> MergedTypeAttributes
        {
            get { return mergedTypeAttributes.ToArray(); }
        }

        public IPropertyBase GetProperty(IPropertyBase sourceProperty, InheritanceMember inheritanceMember)
        {
            if (mappingProperties.ContainsKey(sourceProperty))
            {
                return inheritanceMember == InheritanceMember.Current
                           ? sourceProperty
                           : mappingProperties[sourceProperty];
            }

            return sourceProperty;
        }

        public IOrmAttribute[] GetPropertyTypeAttributes(IPropertyBase sourceProperty)
        {
            if (mappingPropertiesTypeAttributes.ContainsKey(sourceProperty))
            {
                return mappingPropertiesTypeAttributes[sourceProperty];
            }

            return sourceProperty.TypeAttributes;
        }

        public IEnumerable<IPropertyBase> GetInheritedProperties()
        {
            return mappingInheritedProperties.ToArray();
        }

        public static PropertiesBuilder Build(IPersistentType persistentType)
        {
            PropertiesBuilder builder = new PropertiesBuilder(persistentType);
            builder.Prepare();
            return builder;
        }

        private void Prepare()
        {
            thisInterface = persistentType as IInterface;
            if (thisInterface == null)
            {
                return;
            }

            //inheritanceTree = thisInterface.GetInheritanceTree();
            inheritanceTree = InheritanceTreeCache.Get(thisInterface);
            if (!inheritanceTree.TreeRebuilded)
            {
                inheritanceTree.RebuildTree();
            }
            allInheritanceTypes = inheritanceTree.GetFlatList(InheritanceListMode.WholeTree);

            PrepareTypeAttributes();
            PrepareProperties();
            PreparePropertiesTypeAttributes();
        }

        private void PrepareTypeAttributes()
        {
            var allInheritedTypeAttributes = (from innerTypeNode in allInheritanceTypes
                        let innerType = innerTypeNode.Interface
                        select innerType.TypeAttributes.Where(attribute => attribute is IEntityIndex).Cast<IEntityIndex>())
                        .SelectMany(list => list);

            var allCurrentTypeAttributes =
                thisInterface.TypeAttributes.Where(currentTypeAttribute => currentTypeAttribute is IEntityIndex).Cast
                    <IEntityIndex>();

            foreach (IEntityIndex allInheritedTypeAttribute in allInheritedTypeAttributes)
            {
                var inheritedTypeKeyFields = allInheritedTypeAttribute.Fields.KeyFields.Select(inhNode => inhNode.DisplayValue);

                bool canAdd = true;

                foreach (IEntityIndex currentTypeAttribute in  allCurrentTypeAttributes)
                {
                    bool hasSameName = currentTypeAttribute.IndexName.IsCustom() && allInheritedTypeAttribute.IndexName.IsCustom() &&
                                Util.StringEqual(currentTypeAttribute.IndexName.Value, allInheritedTypeAttribute.IndexName.Value, true);

                    canAdd = !hasSameName;

                    if (canAdd)
                    {
                        var currentTypeKeyFields = currentTypeAttribute.Fields.KeyFields.Select(node => node.DisplayValue);
                        canAdd = currentTypeKeyFields.Except(inheritedTypeKeyFields).Count() == inheritedTypeKeyFields.Count();
                    }

                    if (!canAdd)
                    {
                        break;
                    }
                }

                if (canAdd)
                {
                    if (!mergedTypeAttributes.Contains(allInheritedTypeAttribute))
                    {
                        mergedTypeAttributes.Add(allInheritedTypeAttribute);
                    }
                }
            }
        }

        private void PrepareProperties()
        {
            var currentTypeAllProperties = thisInterface.AllProperties;
            foreach (var currentTypeProperty in currentTypeAllProperties)
            {
                IPropertyBase property = currentTypeProperty;

                if (currentTypeProperty.IsInherited)
                {
                    // select all properties from whole inheritance tree which has same type,name and is not marked as 'IsInherited'
                    // these properties will be used as base properties for result properties 
                    // (will be used its ScalarProperty.Type or StructureProperty.TypeOf or NavigationProperty.Assocaitions)
                    var query = from innerTypeNode in allInheritanceTypes
                                let innerType = innerTypeNode.Interface
                                select innerType.AllProperties.Where(item => !item.IsInherited &&
                                item.PropertyKind == property.PropertyKind && Util.StringEqual(item.Name, property.Name, true));
                    
                    // this query in ideal situation must return only 0 or 1 record, but in some cases (where model with validation errors is saved)
                    // it will return more records, then we get only First one.
                    IPropertyBase firstInheritedProperty = query.SelectMany(list => list).FirstOrDefault();
                    
                    mappingProperties[property] = firstInheritedProperty ?? property;
                }
            }

            var allInheritanceEntitiesProp = from inhType in allInheritanceTypes
                    let inhTypeAsEntity = inhType.Interface as IEntityBase
                                             let inhProperties = inhTypeAsEntity == null || !PropertiesBuilderContext.Current.Contains(inhTypeAsEntity)
                                            ? new IPropertyBase[0]
                                            : PropertiesBuilderContext.Current.Get(inhTypeAsEntity).GetInheritedProperties()
                    where inhTypeAsEntity != null //&& inhTypeAsEntity.InheritanceModifier != InheritanceModifiers.Abstract
                                             select inhType.Interface.AllProperties.Concat(inhProperties);

            var allInheritanceEntitiesProperties = allInheritanceEntitiesProp.SelectMany(list => list).ToList();

            //TODO: Tuto musi byt moznost vratit vsetko inheritovany properties (kvoli generovaniu constructorov s parametrami key fieldov!!!)

            // this includes only properties not implemented in current type
            // that means properties from interfaces and from base types of type 'IEntityBase' but only abstract
            var inheritedPropertiesNotInCurrentType =
                from inhType in allInheritanceTypes
                let inhInterface = inhType.Interface
                let inhProperties =
                    from inhProp in inhInterface.AllProperties
                        let inhPropAsEntity = inhProp as IEntityBase
                    where !currentTypeAllProperties.Any(
                        currentTypeProperty =>
                        inhProp.PropertyKind == currentTypeProperty.PropertyKind && Util.StringEqual(inhProp.Name, currentTypeProperty.Name, true)
                        )
                        &&
                        (inhProp.Owner.TypeKind == PersistentTypeKind.Interface ||
                        (inhPropAsEntity != null /*&& inhPropAsEntity.InheritanceModifier == InheritanceModifiers.Abstract*/))
                        &&
                        !allInheritanceEntitiesProperties.Any(ip => ip.PropertyKind == inhProp.PropertyKind && Util.StringEqual(inhProp.Name, ip.Name, true))
                        /*&&
                        !PropertiesBuilderContext.Current.IsInheritedProperty(inhProp.Owner, inhProp)*/
                    select inhProp
                select inhProperties;

            var inhPropertiesList = inheritedPropertiesNotInCurrentType.SelectMany(list => list).ToArray();
            foreach (var inheritedProperty in inhPropertiesList)
            {
                if (!mappingInheritedProperties.Any(item => 
                    item.PropertyKind == inheritedProperty.PropertyKind && Util.StringEqual(item.Name, inheritedProperty.Name, true)))
                {
                    mappingInheritedProperties.Add(inheritedProperty);
                }
            }
        }

        private void PreparePropertiesTypeAttributes()
        {
            mappingPropertiesTypeAttributes.Clear();
            foreach (var mapping in mappingProperties)
            {
                IPropertyBase sourceProperty = mapping.Key;
                IPropertyBase inheritedProperty = mapping.Value;
                
                IOrmAttribute[] typeAttributes = null;
                if (sourceProperty == inheritedProperty)
                {
                    typeAttributes = sourceProperty.TypeAttributes;
                }
                else
                {
                    List<IOrmAttribute> mergedTypeAttributes = new List<IOrmAttribute>();

                    foreach (var typeAttribute in sourceProperty.TypeAttributes)
                    {
                        Type typeOftypeAttribute = typeAttribute.GetType();
                        IOrmAttribute inheritedTypeAttribute = inheritedProperty.TypeAttributes.Single(item => item.GetType() == typeOftypeAttribute);

                        IOrmAttribute mergedTypeAttribute = typeAttribute.MergeChanges(inheritedTypeAttribute, MergeConflictAction.TakeCurrent);
                        mergedTypeAttributes.Add(mergedTypeAttribute);
                    }

                    typeAttributes = mergedTypeAttributes.ToArray();
                }

                mappingPropertiesTypeAttributes.Add(sourceProperty, typeAttributes);
            }
        }
    }
}