using commercetools.Sdk.Domain;
using System;

namespace commercetools.Sdk.Client
{
    public interface IClient
    {
        string Name { get; set; }

        // TODO just an example to start with, will be replaced with generics
        Category GetCategoryById(Guid categoryId);
    }
}