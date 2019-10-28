using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Validation;
using TXSoftware.DataObjectsNetEntityModel.Common;
using TXSoftware.DataObjectsNetEntityModel.Common.Modeling;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    [ValidationState(ValidationState.Enabled)]
    internal partial class Interface : IInterface
    {
        protected override PersistentTypeKind GetTypeKindValue()
        {
            return PersistentTypeKind.Interface;
        }

        protected override void BuildTypeAttributes(List<IOrmAttribute> typeAttributes)
        {
            typeAttributes.AddRange(this.Indexes);
        }

        private string GetInheritInterfacesValue()
        {

            StringBuilder sb = InheritedInterfaces.Select(item => item.Name).Aggregate(new StringBuilder(),
                (builder, s) => builder.AppendFormat("{0},", s));

            return sb.Length == 0 ? string.Empty : sb.Remove(sb.Length - 1, 1).ToString();
        }

        public bool InheritInterface(Interface @interface)
        {
            var interfaces = InterfaceInheritInterfaces.GetInheritedInterfaces(this);
            return interfaces.Any(item => item == @interface);
        }

        #region Implementation of IInterface

        bool IInterface.InheritInterface(IInterface @interface)
        {
            return this.InheritInterface(this);
        }

        ReadOnlyCollection<IInterface> IInterface.InheritedInterfaces
        {
            get
            {
                var items = this.InheritedInterfaces.OfType<Interface>().ToArray();
                ReadOnlyCollection<IInterface> result = new ReadOnlyCollection<IInterface>(items);
                return result;
            }
        }

        ReadOnlyCollection<ITypedEntitySet> IInterface.ReferencedInTypedEntitySets
        {
            get
            {
                var items = this.TypedEntitySets.OfType<TypedEntitySet>().ToArray();
                ReadOnlyCollection<ITypedEntitySet> result = new ReadOnlyCollection<ITypedEntitySet>(items);
                return result;
            }
        }

        ReadOnlyCollection<IEntityIndex> IInterface.Indexes
        {
            get
            {
                var items = this.Indexes.OfType<EntityIndex>().ToArray();
                ReadOnlyCollection<IEntityIndex> result = new ReadOnlyCollection<IEntityIndex>(items);
                return result;
            }
        }

        ReadOnlyCollection<IInterface> IInterface.InheritingByInterfaces
        {
            get
            {
                var items = this.InheritingByInterfaces.OfType<IInterface>().ToArray();
                ReadOnlyCollection<IInterface> result = new ReadOnlyCollection<IInterface>(items);
                return result;
            }
        }

        InheritsIEntityMode IInterface.InheritsIEntity
        {
            get
            {
                return this.InheritsIEntity;
            }
        }


        public void ImplementToType(IEntityBase targetType, ImplementTypeOptions options)
        {
            this.Store.MakeActionWithinTransaction(string.Format("Implementing interface '{0}' to existing type '{1}'",
                this.Name, targetType.Name), () => InternalImplementToType(targetType, options));

        }

        private void InternalImplementToType(IEntityBase targetType, ImplementTypeOptions options)
        {
            EntityBase entityBase = (EntityBase) targetType;

            List<PropertyBase> addedProperties = new List<PropertyBase>();

            foreach (PropertyBase property in this.Properties)
            {
                if (!entityBase.ContainsProperty(property.PropertyKind, property.Name))
                {
                    PropertyBase copiedProperty =
                        (PropertyBase) property.Copy(new[] {PersistentTypeHasProperties.PersistentTypeDomainRoleId});
                    entityBase.Properties.Add(copiedProperty);
                    addedProperties.Add(copiedProperty);
                }
            }

            foreach (NavigationProperty navigationProperty in this.NavigationProperties)
            {
                if (!entityBase.ContainsProperty(PropertyKind.Navigation, navigationProperty.Name))
                {
                    NavigationProperty copiedNavigationProperty = (NavigationProperty)navigationProperty.Copy(new[]
                                    {
                                        PersistentTypeHasNavigationProperties.PersistentTypeOfNavigationPropertyDomainRoleId,
                                    });
                    entityBase.NavigationProperties.Add(copiedNavigationProperty);
                    copiedNavigationProperty.PersistentTypeHasAssociations.SourcePersistentType = entityBase;
                    addedProperties.Add(copiedNavigationProperty);
                }
            }

            foreach (EntityIndex entityIndex in this.Indexes)
            {
                if (!entityBase.ContainsIndex(entityIndex))
                {
                    EntityIndex copiedIndex =
                        (EntityIndex) entityIndex.Copy(new[] {InterfaceHasIndexes.InterfaceOfIndexDomainRoleId});
                    entityBase.Indexes.Add(copiedIndex);
                }
            }

            if (Util.IsFlagSet(ImplementTypeOptions.CopyInheritedInterfaces, options))
            {
                foreach (Interface inheritedInterface in this.InheritedInterfaces)
                {
                    if (!entityBase.InheritedInterfaces.Contains(inheritedInterface))
                    {
                        entityBase.InheritedInterfaces.Add(inheritedInterface);
                    }
                }
            }

            if (!targetType.InheritedInterfaces.Contains(this))
            {
                entityBase.InheritedInterfaces.Add(this);
            }

            var duplicatedInfo = PersistentTypeValidation.FindDuplicatedPropertieInInheritanceTree(targetType, null);
            foreach (var addedProperty in addedProperties)
            {
                Func<IPropertyBase, bool> foundPropertyFunc =
                    item => item.PropertyKind == addedProperty.PropertyKind &&
                            Util.StringEqual(item.Name, addedProperty.Name, true);

                addedProperty.IsInherited =
                    duplicatedInfo.PropertiesWithDifferentType.Any(foundPropertyFunc) ||
                    duplicatedInfo.PropertiesWithSameType.Any(foundPropertyFunc);
            }
        }

        public IEntityBase ImplementToNewType(PersistentTypeKind newTypeKind, string newTypeName, ImplementTypeOptions options)
        {
            if (!newTypeKind.In(PersistentTypeKind.Entity, PersistentTypeKind.Structure))
            {
                throw new ArgumentOutOfRangeException("newTypeKind", "Interface can be implemented only be Entities or Structures!");
            }

            EntityBase newEntity = null;
            this.Store.MakeActionWithinTransaction(string.Format("Implementing new '{0}': {1} from interface '{2}'",
                                                                 newTypeName, newTypeKind, this.Name),
                        delegate
                        {
                            newEntity = newTypeKind == PersistentTypeKind.Entity
                                            ? (EntityBase)new Entity(this.Partition)
                                            : new Structure(this.Partition);

                            newEntity.Access = this.Access;
                            newEntity.Documentation = this.Documentation;
                            newEntity.Name = newTypeName;

                            this.EntityModel.PersistentTypes.Add(newEntity);

                            InternalImplementToType(newEntity, options);

                        });

            return newEntity;
        }

        public bool ContainsProperty(PropertyKind propertyKind, string propertyName)
        {
            return this.Properties.Any(property => property.PropertyKind == propertyKind 
                && Util.StringEqual(property.Name, propertyName, true));
        }

        public bool ContainsIndex(IEntityIndex other)
        {
            bool result = false;

            if (other.IndexName.IsCustom())
            {
                result = this.Indexes.Any(
                    index => index.IndexName.IsCustom() 
                        && Util.StringEqual(index.IndexName.Value, other.IndexName.Value, true));
            }

            if (!result)
            {
                var keyFields = other.Fields.KeyFields.Select(treeNode => treeNode.DisplayValue).ToArray();

                result = this.Indexes.Any(index => index.Fields.KeyFields.Any(
                    node => keyFields.Any(keyField => Util.StringEqual(node.DisplayValue, keyField, true))));
            }

            return result;
        }

        public InheritanceTree GetInheritanceTree()
        {
            return InheritanceTree.Create(this);
        }

        public virtual IEnumerable<IInterface> GetCurrentLevelInheritedInterfaces()
        {
            return this.InheritedInterfaces;
        }

        #endregion

        #region validation 

        [ValidationMethod(ValidationCategories.Menu | ValidationCategories.Save)]
        private void ValidateName(ValidationContext context)
        {
            PersistentTypeValidation.ValidateName(this, context);
        }

        public override ReadOnlyCollection<IPropertyBase> GetAllProperties(bool includeInheritance)
        {
            if (!includeInheritance)
            {
                return base.GetAllProperties(false);
            }

            var inheritanceTree = InheritanceTreeCache.Get(this);
            var  distinctByName = new IdentityProjectionEqualityComparer
                <IPropertyBase, string>(property => property.Name);
            var flatList = inheritanceTree.GetFlatList(InheritanceListMode.WholeTree);
            var inheritanceList = flatList.Reverse().Select((node, i) => new { pos = i, @interface = node.Interface });
            var allProps = (from item in inheritanceList
                            orderby item.pos
                                 select item.@interface.AllProperties).SelectMany(list => list).Concat(this.AllProperties);
            var result = allProps.Distinct(distinctByName).ToList();

            return new ReadOnlyCollection<IPropertyBase>(result);
        }

        #endregion validation
    }
}