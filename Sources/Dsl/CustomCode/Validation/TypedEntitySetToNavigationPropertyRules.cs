using Microsoft.VisualStudio.Modeling;
using TXSoftware.DataObjectsNetEntityModel.Common.Modeling;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl.CustomCode.Rules
{
    public sealed class TypedEntitySetToNavigationPropertyRules
    {
        private const string CODE_TYPED_ENTITYSET_ITEM_TYPE = "TypedEntitySetItemType";
        private const string ERROR_MISSING_ITEM_TYPE = "Typed EntitSet '{0}' has not assigned its Item Type.";
        private const string ERROR_INCOMPATIBLE_ITEM_TYPE = "Typed EntitSet '{0}' pointing to type '{1}' must be same as type '{2}' of navigation end point.";

        #region Rule classes 

        [RuleOn(typeof(NavigationPropertyHasTypedEntitySet), FireTime = TimeToFire.LocalCommit)]
        public sealed class AddRule : Microsoft.VisualStudio.Modeling.AddRule
        {
            public override void ElementAdded(ElementAddedEventArgs e)
            {
                NavigationPropertyHasTypedEntitySet link = e.ModelElement as NavigationPropertyHasTypedEntitySet;
                Validate(link);
            }
        }

        [RuleOn(typeof(NavigationPropertyHasTypedEntitySet), FireTime = TimeToFire.LocalCommit)]
        public sealed class ChangeRule : Microsoft.VisualStudio.Modeling.ChangeRule
        {
            public override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
            {
                NavigationPropertyHasTypedEntitySet link = e.ModelElement as NavigationPropertyHasTypedEntitySet;
                Validate(link);
            }
        }

        [RuleOn(typeof(NavigationPropertyHasTypedEntitySet), FireTime = TimeToFire.LocalCommit)]
        public sealed class RolePlayerChangeRule : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
        {
            public override void RolePlayerChanged(RolePlayerChangedEventArgs e)
            {
                NavigationPropertyHasTypedEntitySet link = e.ElementLink as NavigationPropertyHasTypedEntitySet;
                Validate(link);
            }
        }

        #endregion Rule classes

        internal static ValidationResult Validate(NavigationPropertyHasTypedEntitySet link)
        {
            return Validate(link, true);
        }

        internal static ValidationResult Validate(NavigationPropertyHasTypedEntitySet link, bool workWithTransaction)
        {
            ValidationResult result = new ValidationResult();

            if (link != null && (!workWithTransaction || !link.CurrentTransactionIsSerializing()))
            {
                TypedEntitySet targetTypedEntitySet = link.TypedEntitySet;
                Interface targetItemTypeOfEntitySet = targetTypedEntitySet.ItemType;

                result.IsValid = targetItemTypeOfEntitySet != null;
                if (!result.IsValid)
                {
                    result.ErrorMessage = string.Format(ERROR_MISSING_ITEM_TYPE, targetTypedEntitySet.Name);
                    result.ErrorCode = CODE_TYPED_ENTITYSET_ITEM_TYPE;
                    result.ValidationElements = new[] {targetTypedEntitySet};
                }

                if (result.IsValid)
                {
                    NavigationProperty sourceNavigationProperty = link.OwnerNavigationProperty;
                    PersistentType targetPersistentType =
                        sourceNavigationProperty.PersistentTypeHasAssociations.TargetPersistentType;

                    result.IsValid = targetItemTypeOfEntitySet == targetPersistentType;
                    if (!result.IsValid && targetItemTypeOfEntitySet.TypeKind == PersistentTypeKind.Interface && targetPersistentType is Interface)
                    {
                        Interface targetInterface = (Interface)targetPersistentType;
                        result.IsValid = targetInterface.InheritInterface(targetItemTypeOfEntitySet);
                    }


                    if (!result.IsValid)
                    {
                        result.ValidationElements = new[] {targetItemTypeOfEntitySet};
                        result.ErrorCode = CODE_TYPED_ENTITYSET_ITEM_TYPE;
                        result.ErrorMessage = string.Format(ERROR_INCOMPATIBLE_ITEM_TYPE,
                                targetTypedEntitySet.Name, targetItemTypeOfEntitySet.Name, targetPersistentType.Name);
                    }
                }

                if (!result.IsValid && workWithTransaction)
                {
                    link.RollbackCurrentTransaction(result.ErrorMessage);
                }
            }

            return result;
        }
    }
}