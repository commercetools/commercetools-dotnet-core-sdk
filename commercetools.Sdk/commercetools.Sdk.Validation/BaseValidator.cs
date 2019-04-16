using System;
using FluentValidation;

namespace commercetools.Sdk.Validation
{
    public abstract class BaseValidator<T>: AbstractValidator<T>, IResourceValidator
    {
        public Type ResourceType => typeof(T);

    }
}
