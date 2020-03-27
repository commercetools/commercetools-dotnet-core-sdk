using System;
using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Client
{
    public interface IInStoreCommand
    {
        string StoreKey { get; }

        Type ResourceType { get; }
    }
}