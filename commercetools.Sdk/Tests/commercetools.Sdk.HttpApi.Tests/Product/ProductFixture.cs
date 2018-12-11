using System;
using System.Collections.Generic;
using System.Text;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;

namespace commercetools.Sdk.HttpApi.Tests
{
    public class ProductFixture : ClientFixture, IDisposable
    {
        private ProductTypeFixture productTypeFixture;
        public List<Product> ProductsToDelete { get; private set; }

        public ProductFixture()
        {
            this.ProductsToDelete = new List<Product>();
            this.productTypeFixture = new ProductTypeFixture();
        }

        public void Dispose()
        {            
            IClient commerceToolsClient = this.GetService<IClient>();
            this.ProductsToDelete.Reverse();
            foreach (Product type in this.ProductsToDelete)
            {
                Product deletedType = commerceToolsClient.ExecuteAsync(new DeleteByIdCommand<Product>(new Guid(type.Id), type.Version)).Result;
            }
            this.productTypeFixture.Dispose();
        }

        public ProductDraft GetProductDraft()
        {
            ProductType productType = this.productTypeFixture.CreateProductType();
            this.productTypeFixture.ProductTypesToDelete.Add(productType);
            ProductDraft productDraft = new ProductDraft();
            productDraft.Name = new LocalizedString() { { "en", this.RandomString(4) } };
            productDraft.Slug = new LocalizedString() { { "en", this.RandomString(3) } };
            productDraft.ProductType = new ResourceIdentifier() { Id = productType.Id };
            return productDraft;
        }

        public Product CreateProduct()
        {
            return this.CreateProduct(this.GetProductDraft());
        }

        public Product CreateProduct(ProductDraft productDraft)
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            Product product = commerceToolsClient.ExecuteAsync(new CreateCommand<Product>(productDraft)).Result;
            return product;
        }
    }
}
