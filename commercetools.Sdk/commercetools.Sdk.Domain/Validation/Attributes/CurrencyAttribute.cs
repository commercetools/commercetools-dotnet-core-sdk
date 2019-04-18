using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Validation.Attributes
{
    public class CurrencyAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return ValidationResult.Success;
        }
    }
}
