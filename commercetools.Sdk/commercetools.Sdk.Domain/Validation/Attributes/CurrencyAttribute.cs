using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Registration;

namespace commercetools.Sdk.Domain.Validation.Attributes
{
    public class CurrencyAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ICurrencyValidator validator = ServiceLocator.Current.GetService<ICurrencyValidator>();
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
