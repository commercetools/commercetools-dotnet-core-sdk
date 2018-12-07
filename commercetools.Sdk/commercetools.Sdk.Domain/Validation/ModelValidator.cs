namespace commercetools.Sdk.Domain.Validation
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ModelValidator : IModelValidator
    {
        public IEnumerable<ValidationResult> IsValid(object model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, context, results, true);
            return results;
        }
    }
}
