using commercetools.Sdk.Registration;

namespace commercetools.Sdk.Domain.Validation.Attributes
{
    using Validation;
    using System.ComponentModel.DataAnnotations;

    public class LanguageAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            ICultureValidator cultureValidator = ValidationExtensions.CultureValidator;
            var result = new ValidationResult(this.ErrorMessage);
            if (cultureValidator == null)
            {
                return result;
            }

            bool validationResult = cultureValidator.IsCultureValid(value.ToString());
            return validationResult ? ValidationResult.Success : result;
        }
    }
}
