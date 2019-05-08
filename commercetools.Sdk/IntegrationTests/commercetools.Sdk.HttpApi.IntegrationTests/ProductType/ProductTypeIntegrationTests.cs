using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace commercetools.Sdk.HttpApi.IntegrationTests
{
    [Collection("Integration Tests")]
    public class ProductTypeIntegrationTests : IClassFixture<ProductTypeFixture>
    {
        private readonly ProductTypeFixture productTypeFixture;

        public ProductTypeIntegrationTests(ProductTypeFixture productTypeFixture)
        {
            this.productTypeFixture = productTypeFixture;
        }

        [Fact]
        public void CreateProductType()
        {
            IClient commerceToolsClient = this.productTypeFixture.GetService<IClient>();
            ProductTypeDraft typeDraft = this.productTypeFixture.CreateProductTypeDraft();
            ProductType type = commerceToolsClient.ExecuteAsync(new CreateCommand<ProductType>(typeDraft)).Result;
            this.productTypeFixture.ProductTypesToDelete.Add(type);
            Assert.Equal(typeDraft.Key, type.Key);
        }
    }
}
