using Microsoft.VisualStudio.Modeling.Validation;
using TXSoftware.DataObjectsNetEntityModel.Common.Modeling;
using TXSoftware.DataObjectsNetEntityModel.Dsl.CustomCode.Rules;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal static class TypedEntitySetValidation
    {
        [ValidationMethod(ValidationCategories.Menu | ValidationCategories.Save)]
        internal static void ValidateTypedEntitySet(TypedEntitySet typedEntitySet, ValidationContext context)
        {
            TypedEntitySetHasItemType link = TypedEntitySetHasItemType.GetLinkToItemType(typedEntitySet);

            ValidationResult validationResult = TypedEntitySetHasItemTypeRules.Validate(link);
            context.LogErrorIfAny(validationResult);
        }
    }
}