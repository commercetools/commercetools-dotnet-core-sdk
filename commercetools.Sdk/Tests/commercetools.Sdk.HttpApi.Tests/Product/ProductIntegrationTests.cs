using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace commercetools.Sdk.HttpApi.Tests
{
    public class ProductIntegrationTests : IClassFixture<ProductFixture>
    {
        private ProductFixture productFixture;

        public ProductIntegrationTests(ProductFixture productFixture)
        {
            this.productFixture = productFixture;
        }

        [Fact]
        public void CreateProduct()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            ProductDraft productDraft = this.productFixture.GetProductDraft();
            Product product = commerceToolsClient.ExecuteAsync(new CreateCommand<Product>(productDraft)).Result;
            this.productFixture.ProductsToDelete.Add(product);
            Assert.Equal(productDraft.Name["en"], product.MasterData.Current.Name["en"].ToString());
        }
    }
}
