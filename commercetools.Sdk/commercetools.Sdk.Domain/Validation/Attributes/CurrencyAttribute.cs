using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Validation.Attributes
{
    public class CurrencyAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            ICurrencyValidator validator = ValidationExtensions.CurrencyValidator;
            var result = new ValidationResult(this.ErrorMessage);
            if (validator == null)
            {
                return result;
            }

            bool validationResult = validator.IsCurrencyValid(value.ToString());
            return validationResult ? ValidationResult.Success : result;
        }
    }
}
