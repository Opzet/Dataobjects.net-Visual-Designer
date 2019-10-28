using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;
using Microsoft.VisualStudio.Modeling.Validation;
using TXSoftware.DataObjectsNetEntityModel.Common;
using TXSoftware.DataObjectsNetEntityModel.Common.Modeling;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors;
using TXSoftware.DataObjectsNetEntityModel.Dsl.CustomCode.Rules;
using ElementEventArgs = TXSoftware.DataObjectsNetEntityModel.Common.Modeling.ElementEventArgs;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    [ValidationState(ValidationState.Enabled)]
    internal partial class NavigationProperty : IElementEventsHandler, INavigationProperty
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="store">Store where new element is to be created.</param>
        /// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
        public NavigationProperty(Store store, params PropertyAssignment[] propertyAssignments)
            : this(store != null ? store.DefaultPartitionForClass(DomainClassId) : null, propertyAssignments)
        {
            Initialize();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="partition">Partition where new element is to be created.</param>
        /// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
        public NavigationProperty(Partition partition, params PropertyAssignment[] propertyAssignments)
            : base(partition, propertyAssignments)
        {
            Initialize();
        }

        private string GetReturnTypeValue()
        {
            string result = string.Empty;
            if (this.PersistentTypeHasAssociations == null)
            {
                return result;
            }

            PersistentType ownerPersistentType = this.PersistentTypeOfNavigationProperty;
            bool isSource = this.PersistentTypeHasAssociations.SourcePersistentType == ownerPersistentType;

            PersistentType targetPersistentType = isSource ? this.PersistentTypeHasAssociations.TargetPersistentType : this.PersistentTypeHasAssociations.SourcePersistentType;

            switch (this.Multiplicity)
            {
                case MultiplicityKind.Many:
                    {
                        result = this.TypedEntitySet == null
                                     ? OrmUtils.BuildXtensiveType(OrmType.EntitySet, targetPersistentType.Name)
                                     : string.Format("{0} ({1})", this.TypedEntitySet.Name,

                                                     OrmUtils.BuildXtensiveType(OrmType.EntitySet,
                                                                                this.TypedEntitySet.ItemType == null
                                                                                    ? string.Empty
                                                                                    : this.TypedEntitySet.ItemType.Name));
                        break;
                    }
                case MultiplicityKind.ZeroOrOne:
                case MultiplicityKind.One:
                    {
                        result = string.Format("Instance of {0}", targetPersistentType.Name);
                        break;
                    }
            }

            return result;
        }

        private bool IsSourceWithinAssociation()
        {
            var persistentTypeHasAssociations = this.PersistentTypeHasAssociations;
            PersistentType ownerPersistentType = this.PersistentTypeOfNavigationProperty;
            return persistentTypeHasAssociations == null 
                ? false 
                : persistentTypeHasAssociations.SourcePersistentType == ownerPersistentType;
        }

        protected override PropertyKind GetPropertyKindValue()
        {
            return PropertyKind.Navigation;
        }

        protected override IPersistentType GetOwner()
        {
            return this.PersistentTypeOfNavigationProperty;
        }

        protected override void BuildTypeAttributes(List<IOrmAttribute> typeAttributes)
        {
            typeAttributes.Add(this.KeyAttribute);

            bool isSource = IsSourceWithinAssociation();
            if (this.PersistentTypeHasAssociations != null)
            {
                IPersistentTypeHasAssociations associations = this.PersistentTypeHasAssociations;
                IAssociationInfo association = isSource
                                                   ? associations.TargetAssociation
                                                   : associations.SourceAssociation;
                typeAttributes.Add(association);
            }
        }

        private void Initialize()
        {
            this.KeyAttribute = new OrmKeyAttribute();
        }

        private string GetPairFromValue()
        {
            return GetPairFromToValue(true);
        }

        private string GetPairToValue()
        {
            return GetPairFromToValue(false);
        }

        private string GetPairFromToValue(bool from)
        {
            bool isSource = IsSourceWithinAssociation();
            var associations = this.PersistentTypeHasAssociations;
            if (associations == null)
            {
                return string.Empty;
            }
            
            var endA = isSource ? associations.End2 : associations.End1;
            var endB = isSource ? associations.End1 : associations.End2;
            Defaultable<string> defPairTo = from ? endA.PairTo : endB.PairTo;
            return defPairTo.IsDefault() ? string.Empty : defPairTo.Value;
        }

        protected override void OnCopy(ModelElement sourceElement)
        {
            base.OnCopy(sourceElement);

            NavigationProperty sourceProperty = (NavigationProperty) sourceElement;
            this.TypedEntitySet = sourceProperty.TypedEntitySet;
            this.KeyAttribute = Common.ExtensionMethods.Clone(sourceProperty.KeyAttribute);

            using (ValidationContextRegion.DisableAll())
            {
                // this will add delete which dispose validation region when transaction completes/rolled-back
                /*this.Store.RegisterActionOnTransactionEvent(TransactionEvent.Committed | TransactionEvent.RolledBack,  
                    disabledValidationRegion, (args, disposable) => disposable.Dispose());*/

                if (sourceProperty.PersistentTypeHasAssociations != null)
                {
                    this.PersistentTypeHasAssociations =
                        (PersistentTypeHasAssociations) sourceProperty.PersistentTypeHasAssociations.Copy(
                            new[]
                            {
                                PersistentTypeHasAssociations.TargetPersistentTypeDomainRoleId,
                                PersistentTypeHasAssociations.SourcePersistentTypeDomainRoleId
                            });
                }
            }

            if (this.PersistentTypeHasAssociations != null)
            {
                this.PersistentTypeHasAssociations.TargetPersistentType =
                    sourceProperty.PersistentTypeHasAssociations.TargetPersistentType;

                this.PersistentTypeHasAssociations.Name =
                    AssociationConnectorBuilder.BuildAssociationName(
                        this.PersistentTypeHasAssociations.SourcePersistentType,
                        this.PersistentTypeHasAssociations.TargetPersistentType);
            }
        }

        #region Implementation of IElementEventsHandler

        public void HandleEvent(ElementEventArgs args)
        {
            if (args.EventType == ElementEventType.ElementPropertyChanged)
            {
                Func<Tuple<OrmAssociationEnd, OrmAssociationEnd>> getAssocEnd = () =>
                {
                    PersistentType currentOwner = this.PersistentTypeOfNavigationProperty;
                    bool isSource = this.PersistentTypeHasAssociations.SourcePersistentType == currentOwner;

                    OrmAssociationEnd assocEnd1 = isSource
                                                     ? this.PersistentTypeHasAssociations.End2
                                                     : this.PersistentTypeHasAssociations.End1;
                    OrmAssociationEnd assocEnd2 = isSource
                                                     ? this.PersistentTypeHasAssociations.End1
                                                     : this.PersistentTypeHasAssociations.End2;
                    return new Tuple<OrmAssociationEnd, OrmAssociationEnd>(assocEnd1, assocEnd2);
                };

                var e = args.ElementPropertyChangedEventArgs;
                if (e.DomainProperty.Id == MultiplicityDomainPropertyId)
                {
                    if (this.PersistentTypeHasAssociations != null)
                    {
                        var assocEnds = getAssocEnd();
                        assocEnds.Item1.Multiplicity = (MultiplicityKind)e.NewValue;
                    }
                }
                else if (e.DomainProperty.Id == NameDomainPropertyId)
                {
                    if (this.PersistentTypeHasAssociations != null)
                    {
                        var assocEnds = getAssocEnd();
                        assocEnds.Item2.PairTo.SetAsCustom(e.NewValue as string);
                    }
                }
            }
        }

        #endregion

        #region Validation

        [ValidationMethod(ValidationCategories.Menu | ValidationCategories.Save)]
        private void ValidateTypedEntitySet(ValidationContext context)
        {
            var linkTypedEntitySet = NavigationPropertyHasTypedEntitySet.GetLinkToTypedEntitySet(this);
            if (linkTypedEntitySet != null)
            {
                ValidationResult validationResult = TypedEntitySetToNavigationPropertyRules.Validate(linkTypedEntitySet, false);
                context.LogErrorIfAny(validationResult);
            }

            PropertiesValidation.ValidateNavigationPropertyAssociation(this, context);
        }

        #endregion Validation

        #region Implementation of INavigationProperty

        MultiplicityKind INavigationProperty.Multiplicity
        {
            get { return this.Multiplicity; }
        }

        string INavigationProperty.PairFrom
        {
            get { return this.PairFrom; }
        }

        string INavigationProperty.PairTo
        {
            get { return this.PairTo; }
        }

        IPersistentType INavigationProperty.OwnerPersistentType
        {
            get { return this.PersistentTypeOfNavigationProperty; }
        }

        IPersistentTypeHasAssociations INavigationProperty.PersistentTypeHasAssociations
        {
            get { return this.PersistentTypeHasAssociations; }
        }

        ITypedEntitySet INavigationProperty.TypedEntitySet
        {
            get { return this.TypedEntitySet; }
        }

        OrmKeyAttribute INavigationProperty.KeyAttribute
        {
            get { return this.KeyAttribute; }
        }

        #endregion
    }
}