using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace commercetools.Sdk.HttpApi.IntegrationTests
{
    [Collection("Integration Tests")]
    public class ProductIntegrationTests : IClassFixture<ProductFixture>
    {
        private readonly ProductFixture productFixture;

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
            Assert.Equal(productDraft.Name["en"], product.MasterData.Current.Name["en"]);
        }

        [Fact]
        public void UploadProductImage()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct();
            Stream imageStream = File.OpenRead("Resources/commercetools.png");
            UploadProductImageCommand uploadCommand = new UploadProductImageCommand(new Guid(product.Id), imageStream, "image/png");
            Product updatedProduct = commerceToolsClient.ExecuteAsync(uploadCommand).Result;
            this.productFixture.ProductsToDelete.Add(updatedProduct);
            Assert.NotNull(updatedProduct.MasterData.Staged.MasterVariant.Images[0].Url);
        }
    }
}
