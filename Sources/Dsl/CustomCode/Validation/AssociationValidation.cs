using System.Linq;
using Microsoft.VisualStudio.Modeling;
using TXSoftware.DataObjectsNetEntityModel.Common;
using TXSoftware.DataObjectsNetEntityModel.Common.Modeling;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal static class AssociationValidation
    {
        private const string ERROR_INVALID_NAME = "Type '{0}' already contains association with name '{1}', please use different name.";
        private const string CODE_INVALID_NAME = "InvalidName";

        internal static ValidationResult ValidateAssociationName(PersistentTypeHasAssociations associations)
        {
            return ValidateAssociationName(associations, associations.Name);
        }

        internal static ValidationResult ValidateAssociationName(PersistentTypeHasAssociations associations, string newName)
        {
            ValidationResult result = new ValidationResult();

            var associationsLink =
                PersistentTypeHasAssociations.GetLinksToPersistentTypeAssociations(associations.SourcePersistentType);

            result.IsValid = associationsLink.Count(association => Util.StringEqual(association.Name, newName, true)) <= 1;

            if (!result.IsValid)
            {
                result.ErrorMessage = string.Format(ERROR_INVALID_NAME, associations.SourcePersistentType.Name, newName);
                result.ErrorCode = CODE_INVALID_NAME;
                result.ValidationElements = new ModelElement[] {associations};
            }

            return result;
        }
    }
}