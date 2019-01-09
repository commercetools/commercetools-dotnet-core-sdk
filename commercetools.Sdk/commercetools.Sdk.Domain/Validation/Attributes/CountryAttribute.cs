using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Registration;

namespace commercetools.Sdk.Domain.Validation.Attributes
{
    public class CountryAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            ICountryValidator validator = ServiceLocator.Current.GetService<ICountryValidator>();
            var result = new ValidationResult(this.ErrorMessage);
            if (validator == null)
            {
                return result;
            }

            bool validationResult = validator.IsCountryValid(value.ToString());
            return validationResult ? ValidationResult.Success : result;
        }
    }
}
