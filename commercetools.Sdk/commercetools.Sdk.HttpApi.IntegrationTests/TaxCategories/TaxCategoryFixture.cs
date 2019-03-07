using System;
using System.Collections.Generic;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Categories;

namespace commercetools.Sdk.HttpApi.IntegrationTests.TaxCategories
{
    public class TaxCategoryFixture : ClientFixture, IDisposable
    {
        public List<TaxCategory> TaxCategoriesToDelete { get; private set; }
        
        public TaxCategoryFixture() : base()
        {
            this.TaxCategoriesToDelete = new List<TaxCategory>();
        }

        public void Dispose()
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            this.TaxCategoriesToDelete.Reverse();
            foreach (TaxCategory category in this.TaxCategoriesToDelete)
            {
                TaxCategory deletedCategory = commerceToolsClient.ExecuteAsync(new DeleteByIdCommand<TaxCategory>(new Guid(category.Id), category.Version)).Result;
            }
        }
    }
}