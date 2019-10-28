using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.VisualStudio.Modeling.Validation;
using TXSoftware.DataObjectsNetEntityModel.Common;
using TXSoftware.DataObjectsNetEntityModel.Common.Modeling;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    [ValidationState(ValidationState.Enabled)]
    public partial class EntityModel: IModelRoot
    {
        #region fields 

        private bool validationCalled = false;
        private bool validationSuccess = false;

        #endregion fields

        public bool HasBuildInDomainTypes
        {
            get { return this.DomainTypes.Any(item => item.IsBuildIn); }
        }

        internal void GenerateBuildInDomainTypes()
        {
            if (HasBuildInDomainTypes)
            {
                return;
            }

            using (ValidationContextRegion.DisableAll())
            {
                foreach (var typePair in SystemPrimitiveTypesConverter.TypeIds)
                {
                    Guid typeId = typePair.Key;
                    Type type = typePair.Value;

                    if (!this.DomainTypes.Any(domainType => Util.StringEqual(domainType.Name, type.Name, true) &&
                                                            Util.StringEqual(domainType.Namespace, type.Namespace, true)))
                    {
                        var buildInDomainType = new DomainType(this.Partition);

                        this.DomainTypes.Add(buildInDomainType);
                        buildInDomainType.Name = type.Name;
                        buildInDomainType.Namespace = type.Namespace;
                        buildInDomainType.IsBuildIn = true;
                        buildInDomainType.BuildInID = typeId;
                    }
                }
            }
        }

        #region Implementation of IModelRoot

        string IModelRoot.Namespace
        {
            get
            {
                CheckValidation();

                return this.Namespace;
            }
        }

        ReadOnlyCollection<IPersistentType> IModelRoot.PersistentTypes
        {
            get
            {
                CheckValidation();

                var persistentTypes = this.PersistentTypes.OfType<PersistentType>().ToArray();
                ReadOnlyCollection<IPersistentType> result = new ReadOnlyCollection<IPersistentType>(persistentTypes);
                return result;
            }
        }

        public ReadOnlyCollection<IInterface> TopHierarchyTypes
        {
            get
            {
                CheckValidation();

                var list = this.PersistentTypes.Where(type => type is Interface)
                    .Cast<IInterface>()
                    .Where(@interface => @interface.InheritingByInterfaces.Count == 0)
                    .Where(@interface => !(@interface is IEntityBase) || (@interface as
                                                                          IEntityBase).ReferencesAsBaseType.Count == 0).ToArray();

                return new ReadOnlyCollection<IInterface>(list);
            }
        }

        ReadOnlyCollection<IDomainType> IModelRoot.DomainTypes
        {
            get
            {
                var domainTypes = this.DomainTypes.OfType<IDomainType>().ToArray();
                return new ReadOnlyCollection<IDomainType>(domainTypes);
            }
        }

        ReadOnlyCollection<IDomainType> IModelRoot.BuildInDomainTypes
        {
            get
            {
                var domainTypes = this.DomainTypes.Where(type => type.IsBuildIn).OfType<IDomainType>().ToArray();
                return new ReadOnlyCollection<IDomainType>(domainTypes);
            }
        }

        private void CheckValidation()
        {
            if (!validationCalled)
            {
                Validate(null);
            }

            if (!validationSuccess)
            {
                throw new ApplicationException("Model does not pass some validation(s).");
            }
        }

        public void Validate(string templateVersion)
        {
            if (validationCalled && validationSuccess)
            {
                return;
            }

            try
            {
                // NOTE: value null of templateVersion is workaround for versions 1.0.0.0 and 1.0.1.0 when there is no templateVersion specified
                if (!string.IsNullOrEmpty(templateVersion))
                {
                    if (ModelApp.IsCompatibleWithTemplate(templateVersion))
                    {
                        if (this.PersistentTypes.Count == 0)
                        {
                            throw new InvalidOperationException(
                                "Assigned entity model does not have any persistent types defined!");
                        }
                    }
                    else
                    {
                        throw new ApplicationException(
                            string.Format(
                                "Current model version '{0}' is not compatible with template version '{1}'. Please update T4 template with most current version (e.g. by creating new template from Add Item... Wizard). Do not update manually value of TemplateVersion because you will loose features of new version or event generator stops working.",
                                ModelApp.ApplicationVersion, templateVersion));
                    }

                    validationSuccess = true;
                }
                else
                {
                    validationSuccess = true;
                }
            }
            finally
            {
                validationCalled = true;
            }
        }

        public IDomainType GetDomainType(Guid typeId)
        {
            return DomainTypes.SingleOrDefault(type => type.BuildInID == typeId || type.Id == typeId);
        }

        #endregion

        private IModelRoot GetModelRootValue()
        {
            return this;
        }

        [ValidationMethod(ValidationCategories.Menu | ValidationCategories.Save)]
        private void ValidateInheritanceTree(ValidationContext context)
        {
            EntityModelValidation.ValidateInheritanceTree(this, context);
        }

        private string GetVersionValue()
        {
            return ModelApp.ApplicationVersion.ToString();
        }

        #region class NamespacePropertyHandler

        internal sealed partial class NamespacePropertyHandler
        {
            protected override void OnValueChanged(EntityModel element, string oldValue, string newValue)
            {
                element.Store.MakeActionWithinTransaction(
                    string.Format("Reflect change of namespace name of entity model from old '{0}' to new '{1}'",
                        oldValue, newValue),
                    delegate
                    {
                        string oldValuelowerInvariant = oldValue.ToLowerInvariant();
                        foreach (var persistentType in element.PersistentTypes.Where(item => item.Namespace.ToLowerInvariant().StartsWith(oldValuelowerInvariant)))
                        {
                            if (Util.StringEqual(persistentType.Namespace, oldValue, true))
                            {
                                persistentType.Namespace = newValue;
                            }
                            else
                            {
                                persistentType.Namespace = persistentType.Namespace.Replace(oldValue, newValue);
                            }
                        }
                    });
            }
        }

        #endregion class NamespacePropertyHandler
    }
}