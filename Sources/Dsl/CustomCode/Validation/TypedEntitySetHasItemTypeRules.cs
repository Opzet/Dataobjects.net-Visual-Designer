using Microsoft.VisualStudio.Modeling;
using TXSoftware.DataObjectsNetEntityModel.Common;
using TXSoftware.DataObjectsNetEntityModel.Common.Modeling;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl.CustomCode.Rules
{
    public sealed class TypedEntitySetHasItemTypeRules
    {
        #region Rule classes

        [RuleOn(typeof(TypedEntitySetHasItemType), FireTime = TimeToFire.LocalCommit)]
        public sealed class AddRule : Microsoft.VisualStudio.Modeling.AddRule
        {
            public override void ElementAdded(ElementAddedEventArgs e)
            {
                TypedEntitySetHasItemType link = e.ModelElement as TypedEntitySetHasItemType;
                Validate(link);
            }
        }

        [RuleOn(typeof(TypedEntitySetHasItemType), FireTime = TimeToFire.LocalCommit)]
        public sealed class ChangeRule : Microsoft.VisualStudio.Modeling.ChangeRule
        {
            public override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
            {
                TypedEntitySetHasItemType link = e.ModelElement as TypedEntitySetHasItemType;
                Validate(link);
            }
        }

        [RuleOn(typeof(TypedEntitySetHasItemType), FireTime = TimeToFire.LocalCommit)]
        public sealed class RolePlayerChangeRule : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
        {
            public override void RolePlayerChanged(RolePlayerChangedEventArgs e)
            {
                TypedEntitySetHasItemType link = e.ElementLink as TypedEntitySetHasItemType;
                Validate(link);
            }
        }

        #endregion Rule classes

        internal static ValidationResult Validate(TypedEntitySetHasItemType link)
        {
            return Validate(link, true);
        }

        internal static ValidationResult Validate(TypedEntitySetHasItemType link, bool workWithTransaction)
        {
            ValidationResult result = new ValidationResult();

            if (link != null && (!workWithTransaction || !link.CurrentTransactionIsSerializing()))
            {
                if (link.TypeOfItem != null)
                {
                    if (!link.TypeOfItem.TypeKind.In(PersistentTypeKind.Entity, PersistentTypeKind.Interface))
                    {
                        result.IsValid = false;
                        result.ErrorCode = "InvalidItemType";
                        result.ValidationElements = new[] {link.TypedEntitySet};
                        result.ErrorMessage =
                            string.Format(
                                "Item type of Typed EntitySet '{0}' is set to '{1}' of type '{2}' which is not allowed. Only Entity and Interface are allowed types for Item Type.",
                                link.TypedEntitySet.Name, link.TypeOfItem.Name, link.TypeOfItem.TypeKind);
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