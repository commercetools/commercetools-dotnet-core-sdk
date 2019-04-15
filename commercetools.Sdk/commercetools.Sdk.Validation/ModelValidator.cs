using System;
using System.Collections.Generic;
using commercetools.Sdk.Domain.Validation;
using FluentValidation;

namespace commercetools.Sdk.Validation
{
    public class ModelValidator : IModelValidator
    {
        private IDictionary<Type, IResourceValidator> mapping = new Dictionary<Type, IResourceValidator>();

        public ModelValidator(IEnumerable<IResourceValidator> validators)
        {
            foreach (var validator in validators)
            {
                mapping.Add(validator.ResourceType, validator);
            }
        }

        public void Validate<T>(T input)
        {
            if (mapping.TryGetValue(typeof(T), out IResourceValidator modelValidator))
            {
                var results = modelValidator.Validate(input);
                if (results.Errors.Count == 0) return;

                throw new ValidationException(results.Errors);
            }
        }
    }
}
