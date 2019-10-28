using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TXSoftware.DataObjectsNetEntityModel.Common;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors;
using TXSoftware.DataObjectsNetEntityModel.Dsl.CustomCode;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal partial class DomainType : IDomainType
    {
        private string GetFullNameValue()
        {
            return string.Format("{0}.{1}", this.Namespace, this.Name);
        }

        private void ValidateNameOrNamespaceChange(string newValue, bool nameChanged)
        {
            string currentFullName = string.Format("{0}.{1}",
                nameChanged ? this.Namespace : newValue,
                nameChanged ? newValue: this.Name);

            bool foundDuplicate = this.EntityModel.DomainTypes.Any(domainType => domainType != this && Util.StringEqual(domainType.FullName,
                currentFullName, true));

            if (foundDuplicate)
            {
                throw new ArgumentException(string.Format("The domain type '{0}' already exists in entity model!", currentFullName));
            }
        }

        #region implementation of IDomainType

        string IElement.Documentation
        {
            get { return string.Empty; }
        }

        string IDomainType.Namespace
        {
            get { return this.Namespace; }
            set { this.Namespace = value; }
        }

        string IDomainType.FullName
        {
            get { return this.FullName; }
        }

        bool IDomainType.IsBuildIn
        {
            get { return this.IsBuildIn; }
        }

        IModelRoot IDomainType.EntityModel
        {
            get { return this.EntityModel; }
        }

        Guid IDomainType.BuildInID
        {
            get { return this.BuildInID; }
            set { this.BuildInID = value; }
        }

        Guid IDomainType.GetTypeId()
        {
            return IsBuildIn ? BuildInID : this.Id;
        }

        public Type TryGetClrType(Type defaultType)
        {
            Type result = null;

            string displayName = SystemPrimitiveTypesConverter.GetDisplayName(this.FullName);
            if (!string.IsNullOrEmpty(displayName))
            {
                result = SystemPrimitiveTypesConverter.GetClrType(displayName);
            }

            return result ?? defaultType;
        }

        public bool EqualsTo(IDomainType other)
        {
            return this.IsBuildIn == other.IsBuildIn &&
                   this.BuildInID == other.BuildInID &&
                   this.FullName == other.FullName;
        }

        #endregion implementation of IDomainType

        #region properties changes handlers

        internal sealed partial class NamePropertyHandler
        {
            protected override void OnValueChanged(DomainType element, string oldValue, string newValue)
            {
                if (!element.Store.InUndoRedoOrRollback && !element.IsValidationDisabled())
                {
                    // Hard validation of the new name.
                    if (string.IsNullOrEmpty(newValue))
                    {
                        throw new ArgumentException("The Name is required and cannot be an empty string.", newValue);
                    }

                    element.ValidateNameOrNamespaceChange(newValue, true);
                }

                // Always call the base class method.
                base.OnValueChanged(element, oldValue, newValue);

            }
        }

        internal sealed partial class NamespacePropertyHandler
        {
            protected override void OnValueChanged(DomainType element, string oldValue, string newValue)
            {
                if (!element.Store.InUndoRedoOrRollback && !element.IsValidationDisabled())
                {
                    // Hard validation of the new name.
                    if (string.IsNullOrEmpty(newValue))
                    {
                        throw new ArgumentException("The Namespace is required and cannot be an empty string.", newValue);
                    }

                    element.ValidateNameOrNamespaceChange(newValue, false);
                }

                // Always call the base class method.
                base.OnValueChanged(element, oldValue, newValue);

            }
        }

        #endregion properties changes handlers

        public override string ToString()
        {
            return this.FullName;
        }

        protected override void OnDeleted()
        {
            base.OnDeleted();
            var query = from entity in this.Store.GetEntityModel().PersistentTypes
                    let scalarProps = entity.GetScalarProperties().Where(prop => prop.Type == this)
                        select scalarProps;
            var scalarProperties = query.SelectMany(list => list);
            foreach (IScalarProperty scalarProperty in scalarProperties)
            {
                (scalarProperty as ScalarProperty).Type = null;
            }
        }
    }
}