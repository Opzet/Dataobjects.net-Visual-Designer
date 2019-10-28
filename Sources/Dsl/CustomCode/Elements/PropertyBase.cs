using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Validation;
using TXSoftware.DataObjectsNetEntityModel.Common;
using System.Linq;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    [ValidationState(ValidationState.Enabled)]
    internal abstract partial class PropertyBase : IPropertyBase
    {
        public string GetTypeNameValue()
        {
            return GetType().Name;
        }

        #region constructors 

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="partition">Partition where new element is to be created.</param>
        /// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
        protected PropertyBase(Partition partition, PropertyAssignment[] propertyAssignments)
            : base(partition, propertyAssignments)
        {
            this.Initialize();
        }

        #endregion constructors

        private void Initialize()
        {
            this.FieldAttribute = new OrmFieldAttribute();
            this.Constraints = new OrmPropertyConstraints();
            this.DataMember = new DataMemberDescriptor();
            this.PropertyAccess = new PropertyAccessModifiers();
        }

        protected abstract void BuildTypeAttributes(List<IOrmAttribute> typeAttributes);

        protected abstract PropertyKind GetPropertyKindValue();

        protected override void OnCopy(ModelElement sourceElement)
        {
            base.OnCopy(sourceElement);

            PropertyBase sourceProperty = (PropertyBase)sourceElement;

            this.FieldAttribute = sourceProperty.FieldAttribute.Clone();

            this.Constraints = sourceProperty.Constraints.Clone();

            this.PropertyAccess = sourceProperty.PropertyAccess.Clone();
        }

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

        PropertyAccessModifiers IPropertyBase.PropertyAccess
        {
            get { return this.PropertyAccess; }
        }

        #endregion

        #region Implementation of IPropertyBase

        IPersistentType IPropertyBase.Owner
        {
            get
            {
                IPersistentType owner = GetOwner();
                IPersistentType impersonatedOwner = PropertyOwnerContext.GetImpersonatedOwner(this);
                return impersonatedOwner ?? owner;
            }
        }

        IPersistentType IPropertyBase.GetRealOwner()
        {
            return GetOwner();
        }

        protected virtual IPersistentType GetOwner()
        {
            return this.PersistentType;
        }

        PropertyKind IPropertyBase.PropertyKind
        {
            get { return this.PropertyKind; }
        }

        OrmFieldAttribute IPropertyBase.FieldAttribute
        {
            get { return this.FieldAttribute; }
        }

        OrmPropertyConstraints IPropertyBase.Constraints
        {
            get { return this.Constraints;  }
        }

        IOrmAttribute[] IPropertyBase.TypeAttributes
        {
            get
            {
                var typeAttributes = new List<IOrmAttribute>
                                     {
                                         FieldAttribute
                                     };

                typeAttributes.AddRange(this.Constraints.AllConstraints);

                BuildTypeAttributes(typeAttributes);
                return typeAttributes.ToArray();
            }
        }

        bool IPropertyBase.IsInherited
        {
            get { return this.IsInherited; }
        }

        DataMemberDescriptor IPropertyBase.DataMember
        {
            get { return this.DataMember; }
        }

        public bool IsImplementedBy(IInterface @interface)
        {
            bool result = false;

            if (@interface != null)
            {
                switch (this.PropertyKind)
                {
                    case PropertyKind.Scalar:
                        {
                            IScalarProperty thisProperty = (IScalarProperty) this;

                            result = @interface.Properties.Any(delegate(IPropertyBase propertyItem)
                                {
                                    IScalarProperty scalarProperty = (IScalarProperty) propertyItem;

                                    return Util.StringEqual(thisProperty.Name, scalarProperty.Name, true) &&
                                        thisProperty.Type.EqualsTo(scalarProperty.Type);
                                });

                            break;
                        }
                    case PropertyKind.Structure:
                        {
                            IStructureProperty thisProperty = (IStructureProperty) this;
                            result = @interface.Properties.Any(delegate(IPropertyBase propertyItem)
                                {
                                    IStructureProperty structureProperty = (IStructureProperty)propertyItem;

                                    return Util.StringEqual(thisProperty.Name, structureProperty.Name, true) &&
                                        thisProperty.TypeOf == structureProperty.TypeOf &&
                                        thisProperty.TypeOf != null;
                                });

                            break;
                        }
                    case PropertyKind.Navigation:
                        {
                            INavigationProperty thisProperty = (INavigationProperty) this;
                            result = @interface.NavigationProperties.Any(delegate(INavigationProperty navigationProperty)
                                {
                                    var thisAssociations = thisProperty.PersistentTypeHasAssociations;

                                    return Util.StringEqual(thisProperty.Name, navigationProperty.Name, true) &&
                                        thisAssociations.TargetPersistentType == navigationProperty.PersistentTypeHasAssociations.TargetPersistentType &&
                                        thisAssociations.TargetPersistentType != null;
                                });

                            break;
                        }
                }
            }

            return result;
        }

        #endregion

        [ValidationMethod(ValidationCategories.Menu | ValidationCategories.Save)]
        private void ValidateName(ValidationContext context)
        {
            PropertiesValidation.ValidateName(this, context);
        }
    }
}