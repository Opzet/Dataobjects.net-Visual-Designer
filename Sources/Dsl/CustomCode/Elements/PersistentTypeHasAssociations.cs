using System;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Validation;
using TXSoftware.DataObjectsNetEntityModel.Common;
using TXSoftware.DataObjectsNetEntityModel.Common.Modeling;
using ElementEventArgs = TXSoftware.DataObjectsNetEntityModel.Common.Modeling.ElementEventArgs;
using System.Linq;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    [ValidationState(ValidationState.Enabled)]
    internal partial class PersistentTypeHasAssociations : IElementEventsHandler, IPersistentTypeHasAssociations
    {
        /// <summary>
        /// Constructor
        /// Creates a PersistentTypeHasAssociations link in the same Partition as the given PersistentType
        /// </summary>
        /// <param name="source">PersistentType to use as the source of the relationship.</param>
        /// <param name="target">PersistentType to use as the target of the relationship.</param>
        public PersistentTypeHasAssociations(PersistentType source, PersistentType target)
        	: base((source != null ? source.Partition : null), new[]{new RoleAssignment(SourcePersistentTypeDomainRoleId, source), 
                new RoleAssignment(TargetPersistentTypeDomainRoleId, target)}, null)
        {
            Initialize();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="store">Store where new link is to be created.</param>
        /// <param name="roleAssignments">List of relationship role assignments.</param>
        public PersistentTypeHasAssociations(Store store, params RoleAssignment[] roleAssignments)
            : base(store != null ? store.DefaultPartitionForClass(DomainClassId) : null, roleAssignments, null)
        {
            Initialize();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="store">Store where new link is to be created.</param>
        /// <param name="roleAssignments">List of relationship role assignments.</param>
        /// <param name="propertyAssignments">List of properties assignments to set on the new link.</param>
        public PersistentTypeHasAssociations(Store store, RoleAssignment[] roleAssignments, PropertyAssignment[] propertyAssignments)
            : base(store != null ? store.DefaultPartitionForClass(DomainClassId) : null, roleAssignments, propertyAssignments)
        {
            Initialize();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="partition">Partition where new link is to be created.</param>
        /// <param name="roleAssignments">List of relationship role assignments.</param>
        public PersistentTypeHasAssociations(Partition partition, params RoleAssignment[] roleAssignments)
            : base(partition, roleAssignments, null)
        {
            Initialize();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="partition">Partition where new link is to be created.</param>
        /// <param name="roleAssignments">List of relationship role assignments.</param>
        /// <param name="propertyAssignments">List of properties assignments to set on the new link.</param>
        public PersistentTypeHasAssociations(Partition partition, RoleAssignment[] roleAssignments, PropertyAssignment[] propertyAssignments)
            : base(partition, roleAssignments, propertyAssignments)
        {
            Initialize();
        }

        private void Initialize()
        {
            this.End1 = new OrmAssociationEnd(this, "End1");
            this.End2 = new OrmAssociationEnd(this, "End2");
        }

        private string MultiplicityToString(MultiplicityKind multiplicityKind)
        {
            switch (multiplicityKind)
            {
                case MultiplicityKind.ZeroOrOne:
                    return "0..1";
                case MultiplicityKind.Many:
                    return "*";
            }

            return "1";
        }

        private string GetSourceMultiplicityValue()
        {
            return MultiplicityToString(End1.Multiplicity);
        }

        private string GetTargetMultiplicityValue()
        {
            return MultiplicityToString(End2.Multiplicity);
        }

        protected override void OnCopy(ModelElement sourceElement)
        {
            base.OnCopy(sourceElement);

            PersistentTypeHasAssociations sourceAssoc = (PersistentTypeHasAssociations) sourceElement;

            this.End1 = sourceAssoc.End1.Clone();
            this.End1.AssignInternalsFrom(sourceAssoc.End1);

            this.End2 = sourceAssoc.End2.Clone();
            this.End2.AssignInternalsFrom(sourceAssoc.End2);
        }

        #region class NamePropertyHandler

        internal sealed partial class NamePropertyHandler
        {
            protected override void OnValueChanged(PersistentTypeHasAssociations element, string oldValue, string newValue)
            {
                if (!element.Store.InUndoRedoOrRollback && !element.IsValidationDisabled())
                {
                    // Hard validation of the new name.
                    if (string.IsNullOrEmpty(newValue))
                    {
                        throw new ArgumentException("The Name is required and cannot be an empty string.", newValue);
                    }

                    var validationResult = AssociationValidation.ValidateAssociationName(element, newValue);

                    if (!validationResult.IsValid)
                    {
                        throw new ArgumentException(validationResult.ErrorMessage);
                    }

                    // Raise the NameChanged event for derived classes to act upon.
                    //element.OnNameChanged(EventArgs.Empty);
                }

                // Always call the base class method.
                base.OnValueChanged(element, oldValue, newValue);
            }
        }

        #endregion class NamePropertyHandler

        [ValidationMethod(ValidationCategories.Menu | ValidationCategories.Save)]
        private void ValidateDuplicatedPropertieInInheritanceTree(ValidationContext context)
        {
            if (!this.IsValidationDisabled())
            {
                var validationResult = AssociationValidation.ValidateAssociationName(this);
                context.LogErrorIfAny(validationResult);
            }
        }

        #region Implementation of IElementEventsHandler

        public void HandleEvent(ElementEventArgs args)
        {
            if (args.EventType == ElementEventType.CustomEvent)
            {
                object customEventArgs = args.CustomEventArgs[0];
                OrmAssociationEnd eventFromAssocEnd = customEventArgs as OrmAssociationEnd;
                string changedProperty = (string) args.CustomEventArgs[1];
                string calledFromEndId = (string) args.CustomEventArgs[2];

                if (eventFromAssocEnd != null)
                {
                    bool changeAll = string.IsNullOrEmpty(changedProperty);
                    bool canChangeMultiplicity = changeAll || Util.StringEqual(changedProperty, "Multiplicity", true);
                    bool canChangePairTo = changeAll || Util.StringEqual(changedProperty, "PairTo", true);

                    bool eventFromEnd1 = Util.StringEqual(calledFromEndId, "End1", true);

                    Func<bool, Tuple<NavigationProperty, OrmAssociationEnd>> getDefinitions = delegate(bool revert)
                    {
                        PersistentType persistentToFind = eventFromEnd1
                                                              ? (revert
                                                                     ? this.SourcePersistentType
                                                                     : this.TargetPersistentType)
                                                              : (revert
                                                                     ? this.TargetPersistentType
                                                                     : this.SourcePersistentType);

                        OrmAssociationEnd otherAssocEnd = eventFromEnd1
                                                              ? (revert 
                                                                    ? this.End2
                                                                    : eventFromAssocEnd)
                                                              : (revert
                                                                    ? eventFromAssocEnd 
                                                                    : this.End2);

                        NavigationProperty navigationProperty = this.NavigationProperties.SingleOrDefault(
                            navProperty => navProperty.PersistentTypeOfNavigationProperty == persistentToFind);

                        return new Tuple<NavigationProperty, OrmAssociationEnd>(navigationProperty, otherAssocEnd);
                    };


                    {
                        if (canChangeMultiplicity)
                        {
                            bool revert = false;
                            var definitions = getDefinitions(revert);
                            var navigationProperty = definitions.Item1;
                            var otherAssocEnd = definitions.Item2;

                            if (navigationProperty != null)
                            {
                                this.Store.MakeActionWithinTransaction(
                                    string.Format("Updating multiplicity on navigation property '{0}' to value '{1}'",
                                                  navigationProperty.Name, eventFromAssocEnd.Multiplicity),
                                    () =>
                                    {
                                        navigationProperty.Multiplicity = otherAssocEnd.Multiplicity;
                                    });
                            }
                        }

                        if (canChangePairTo && !eventFromAssocEnd.PairTo.IsDefault())
                        {
                            string newName = eventFromAssocEnd.PairTo.Value;

                            bool revert = true;
                            var definitions = getDefinitions(revert);
                            var navigationProperty = definitions.Item1;

                            if (navigationProperty != null)
                            {
                                this.Store.MakeActionWithinTransaction(
                                    string.Format("Updating name on navigation property '{0}' to value '{1}'",
                                                  navigationProperty.Name, newName),
                                    () =>
                                    {
                                        navigationProperty.Name = newName;
                                    });
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Implementation of IElement

        string IElement.Name
        {
            get { return this.namePropertyStorage; }
            set { namePropertyStorage = value; }
        }

        string IElement.Documentation
        {
            get { return string.Empty; }
        }

        #endregion

        #region Implementation of IPersistentTypeHasAssociations

        IAssociationInfo IPersistentTypeHasAssociations.SourceAssociation
        {
            get { return new AssociationInfo(this.End1);}
        }

        IAssociationInfo IPersistentTypeHasAssociations.TargetAssociation
        {
            get { return new AssociationInfo(this.End2); }
        }

        ReadOnlyCollection<INavigationProperty> IPersistentTypeHasAssociations.NavigationProperties
        {
            get
            {
                var navigationProperties = this.NavigationProperties.OfType<NavigationProperty>().ToArray();
                ReadOnlyCollection<INavigationProperty> result = new ReadOnlyCollection<INavigationProperty>(navigationProperties);
                return result;
            }
        }

        IPersistentType IPersistentTypeHasAssociations.SourcePersistentType
        {
            get { return this.SourcePersistentType; }
        }

        IPersistentType IPersistentTypeHasAssociations.TargetPersistentType
        {
            get { return this.TargetPersistentType; }
        }

        bool IPersistentTypeHasAssociations.EqualAssociationLinkTo(IPersistentTypeHasAssociations other)
        {
            return this.SourcePersistentType == other.SourcePersistentType &&
                   this.TargetPersistentType == other.TargetPersistentType;
        }

        #endregion
    }
}