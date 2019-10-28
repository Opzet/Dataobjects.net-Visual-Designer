using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.VisualStudio.Modeling.Validation;
using TXSoftware.DataObjectsNetEntityModel.Common;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    [ValidationState(ValidationState.Enabled)]
    internal partial class TypedEntitySet : ITypedEntitySet
    {
        protected override PersistentTypeKind GetTypeKindValue()
        {
            return PersistentTypeKind.TypedEntitySet;
        }

        protected override string GetTypeDescriptionValue()
        {
            return OrmUtils.BuildXtensiveType(OrmType.EntitySet, this.ItemType == null ? "?" : this.ItemType.Name);
        }

        protected override void BuildTypeAttributes(List<IOrmAttribute> typeAttributes)
        {
            
        }

        #region Validation

        [ValidationMethod(ValidationCategories.Menu | ValidationCategories.Save)]
        private void ValidateTypedEntitySet(ValidationContext context)
        {
            TypedEntitySetValidation.ValidateTypedEntitySet(this, context);
        }

        [ValidationMethod(ValidationCategories.Menu | ValidationCategories.Save)]
        private void ValidateName(ValidationContext context)
        {
            PersistentTypeValidation.ValidateName(this, context);
        }

        #endregion Validation

        #region Implementation of ITypedEntitySet

        IInterface ITypedEntitySet.ItemType
        {
            get { return this.ItemType; }
        }

        ReadOnlyCollection<INavigationProperty> ITypedEntitySet.NavigationPropertiesReferencingThis
        {
            get
            {
                var navigationProperties = this.NavigationProperties.OfType<NavigationProperty>().ToArray();
                ReadOnlyCollection<INavigationProperty> result = new ReadOnlyCollection<INavigationProperty>(navigationProperties);
                return result;
            }
        }

        #endregion
    }
}