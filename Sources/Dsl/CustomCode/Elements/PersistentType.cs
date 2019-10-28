using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Validation;
using TXSoftware.DataObjectsNetEntityModel.Common;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors;
using TXSoftware.DataObjectsNetEntityModel.Dsl.CustomCode;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    [ValidationState(ValidationState.Enabled)]
    internal partial class PersistentType : IPersistentType
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="partition">Partition where new element is to be created.</param>
        /// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
        protected PersistentType(Partition partition, PropertyAssignment[] propertyAssignments)
            : base(partition, propertyAssignments)
        {
            this.Initialize();
        }

        private void Initialize()
        {
            this.DataContract = new DataContractDescriptor();
        }

        public string GetTypeNameValue()
        {
            return GetType().Name;
        }

        protected abstract PersistentTypeKind GetTypeKindValue();

        private bool GetInheritsFromValue()
        {
            int inheritCount = 0;

            Interface @interface = this as Interface;
            if (@interface != null)
            {
                inheritCount += @interface.InheritedInterfaces.Count;
            }
            
            Entity entity = this as Entity;
            if (entity != null && entity.BaseType != null)
            {
                inheritCount += 1;
            }

            return inheritCount > 0;
        }

        private string GetInheritsFromNameValue()
        {
            List<string> inheritList = new List<string>();
            Interface @interface = this as Interface;
            if (@interface != null)
            {
                inheritList.AddRange(@interface.InheritedInterfaces.Select(item => item.Name).ToArray());
            }

            Entity entity = this as Entity;
            if (entity != null && entity.BaseType != null)
            {
                inheritList.Add(entity.BaseType.Name);
            }

            return
                inheritList.Aggregate(new StringBuilder(), (builder, s) => builder.AppendFormat("{0},", s)).ToString();
        }

        protected virtual string GetTypeDescriptionValue()
        {
            string result = string.Empty;

            if (InheritsFrom)
            {
                result = InheritsFromName;
            }
            else
            {
                switch (TypeKind)
                {
                    case PersistentTypeKind.Interface:
                    case PersistentTypeKind.Entity:
                    case PersistentTypeKind.Structure:
                        {
                            result = TypeKind.ToString();
                            break;
                        }
                    case PersistentTypeKind.ExternalType:
                        {
                            result = "External Type";
                            break;
                        }
                    case PersistentTypeKind.TypedEntitySet:
                        {
                            result = "Typed EntitySet";
                            break;
                        }
                }
            }

            return result;
        }

        public ScalarProperty AddScalarProperty(string name, Type type)
        {
            Guid typeId = SystemPrimitiveTypesConverter.GetTypeId(type);
            IDomainType domainType = null;
            IModelRoot entityModel = this.Store.GetEntityModel();
            if (typeId != default(Guid))
            {
                domainType = entityModel.GetDomainType(typeId);
            }
            else
            {
                domainType = entityModel.DomainTypes.SingleOrDefault(item => item.FullName == type.FullName);
            }

            if (domainType != null)
            {
                return AddScalarProperty(name, (DomainType) domainType);
            }

            throw new ArgumentOutOfRangeException("type is not registered as Domain Type!");
        }

        public ScalarProperty AddScalarProperty(string name, DomainType type)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            if (this.GetScalarProperties().Any(item => Util.StringEqual(item.Name, name, true)))
            {
                throw new ApplicationException(string.Format("Scalar property with name '{0}' already exist.",
                    name));
            }

            ScalarProperty newProperty = new ScalarProperty(this.Partition);
            newProperty.Type = type;//SystemPrimitiveTypesConverter.GetDisplayName(type);
            this.Properties.Add(newProperty);
            return newProperty;
        }

        protected abstract void BuildTypeAttributes(List<IOrmAttribute> typeAttributes);

        #region validation and changed events for property 'Name'

        /// <summary>
        /// Occurs when the name of the ConfigurationElement changed.
        /// </summary>
        protected event EventHandler<EventArgs> NameChanged;

        /// <summary>
        /// Raises the <see cref="E:NameChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnNameChanged(EventArgs e)
        {
            if (this.NameChanged != null)
            {
                this.NameChanged(this, EventArgs.Empty);
            }
        }

        #region class NamePropertyHandler

        internal sealed partial class NamePropertyHandler
        {
            protected override void OnValueChanged(PersistentType element, string oldValue, string newValue)
            {
                if (!element.Store.InUndoRedoOrRollback)
                {
                    // Hard validation of the new name.
                    if (string.IsNullOrEmpty(newValue))
                    {
                        throw new ArgumentException("The Name is required and cannot be an empty string.", newValue);
                    }

                    // Raise the NameChanged event for derived classes to act upon.
                    element.OnNameChanged(EventArgs.Empty);
                }

                // Always call the base class method.
                base.OnValueChanged(element, oldValue, newValue);

            }
        }

        #endregion class NamePropertyHandler

        [ValidationMethod(ValidationCategories.Menu | ValidationCategories.Save)]
        private void ValidateName(ValidationContext context)
        {
            PersistentTypeValidation.ValidateName(this, context);
        }

        [ValidationMethod(ValidationCategories.Menu | ValidationCategories.Save)]
        private void ValidateProperties(ValidationContext context)
        {
            if (!context.IsInGlobalStage(ValidationGlobalStage.EntityModelInheritanceTree))
            {
                PersistentTypeValidation.ValidateProperties(this, null, context);
            }
        }

        #endregion validation and changed events for property 'Name'

        #region Implementation of IAccessibleElement

        string IElement.Name
        {
            get { return this.namePropertyStorage; }
            set { this.namePropertyStorage = value; }
        }

        string IElement.Documentation
        {
            get { return this.Documentation; }
        }

        AccessModifier IAccessibleElement.Access
        {
            get { return this.Access; }
        }

        #endregion

        #region Implementation of IPersistentType

        string IPersistentType.Namespace
        {
            get { return this.namespacePropertyStorage; }
            set { this.namespacePropertyStorage = value; }
        }

        PersistentTypeKind IPersistentType.TypeKind
        {
            get { return this.TypeKind; }
        }

        ReadOnlyCollection<IPropertyBase> IPersistentType.Properties
        {
            get
            {
                var properties = this.Properties.OfType<PropertyBase>().ToArray();
                ReadOnlyCollection<IPropertyBase> result = new ReadOnlyCollection<IPropertyBase>(properties);
                return result;
            }
        }

        ReadOnlyCollection<INavigationProperty> IPersistentType.NavigationProperties
        {
            get
            {
                var navigationProperties = this.NavigationProperties.OfType<NavigationProperty>().ToArray();
                ReadOnlyCollection<INavigationProperty> result = new ReadOnlyCollection<INavigationProperty>(navigationProperties);
                return result;
            }
        }

        public ReadOnlyCollection<IPropertyBase> AllProperties
        {
            get
            {
                var scalarProperties = this.Properties.OfType<IPropertyBase>().ToArray();
                var navigationProperties = this.NavigationProperties.OfType<IPropertyBase>().ToArray();
                var allProperties = scalarProperties.Concat(navigationProperties).ToList();
                ReadOnlyCollection<IPropertyBase> result = new ReadOnlyCollection<IPropertyBase>(allProperties);
                return result;
            }
        }

        ReadOnlyCollection<IPersistentType> IPersistentType.PersistentTypeAssociations
        {
            get
            {
                var persistentTypes = this.PersistentTypeAssociations.OfType<PersistentType>().ToArray();
                ReadOnlyCollection<IPersistentType> result = new ReadOnlyCollection<IPersistentType>(persistentTypes);
                return result;
            }
        }

        IOrmAttribute[] IPersistentType.TypeAttributes
        {
            get
            {
                var typeAttributes = new List<IOrmAttribute>();
                BuildTypeAttributes(typeAttributes);
                return typeAttributes.ToArray();
            }
        }

        DataContractDescriptor IPersistentType.DataContract
        {
            get { return this.DataContract; }
        }

        public virtual ReadOnlyCollection<IPropertyBase> GetAllProperties(bool includeInheritance)
        {
            return AllProperties;
        }

        public ReadOnlyCollection<IScalarProperty> GetScalarProperties()
        {
            var list = this.Properties.Where(prop => prop is IScalarProperty).Cast<IScalarProperty>().ToArray();
            return new ReadOnlyCollection<IScalarProperty>(list);
        }

        public ReadOnlyCollection<IStructureProperty> GetStructureProperties()
        {
            var list = this.Properties.Where(prop => prop is IStructureProperty).Cast<IStructureProperty>().ToArray();
            return new ReadOnlyCollection<IStructureProperty>(list);
        }

//        public IPropertiesBuilder GetPropertiesBuilder()
//        {
//            return PropertiesBuilderContext.Current.Get(this);
            //return PropertiesBuilder.Build(this);
//        }

        #endregion
    }
}