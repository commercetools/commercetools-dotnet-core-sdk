using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Validation.Attributes
{
    public class SlugAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // TODO Implement
            return ValidationResult.Success;
        }
    }
}
