using System;
using FluentValidation;

namespace commercetools.Sdk.Validation
{
    public interface IResourceValidator : IValidator
    {
        Type ResourceType { get; }
    }
}
