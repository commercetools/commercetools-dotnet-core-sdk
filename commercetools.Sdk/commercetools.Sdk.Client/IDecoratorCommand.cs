using System;

namespace commercetools.Sdk.Client
{
    public interface IDecoratorCommand
    {
        Type ResultType { get; }
    }
}