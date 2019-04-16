using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Registration;

namespace commercetools.Sdk.Domain.Validation.Attributes
{
    public class CountryAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return ValidationResult.Success;
        }
    }
}
