namespace commercetools.Sdk.Domain.Validation.Attributes
{
    using Validation;
    using Util;
    using System.ComponentModel.DataAnnotations;

    public class LanguageAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ICultureValidator cultureValidator = ServiceLocator.Current.GetService<ICultureValidator>();
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
