using Microsoft.VisualStudio.Modeling;

namespace TXSoftware.DataObjectsNetEntityModel.Common.Modeling
{
    public sealed class ValidationResult
    {
        public bool IsValid { get; set; }

        public string ErrorMessage { get; set; }

        public string ErrorCode { get; set; }

        public ModelElement[] ValidationElements { get; set; }

        public ValidationResult()
        {
            this.IsValid = true;
        }

        public ValidationResult(string errorMessage, string errorCode) : this(errorMessage, errorCode, new ModelElement[0])
        {}

        public ValidationResult(string errorMessage, string errorCode, ModelElement[] validationElements)
        {
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
            ValidationElements = validationElements;
        }
    }
}