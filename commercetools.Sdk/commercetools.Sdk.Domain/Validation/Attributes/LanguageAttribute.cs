using commercetools.Sdk.Registration;

namespace commercetools.Sdk.Domain.Validation.Attributes
{
    using Validation;
    using System.ComponentModel.DataAnnotations;

    public class LanguageAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return ValidationResult.Success;
        }
    }
}
