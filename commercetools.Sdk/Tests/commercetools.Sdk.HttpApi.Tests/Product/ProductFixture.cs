using System;
using System.Collections.Generic;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Products;
using commercetools.Sdk.Domain.Products.Attributes;
using Attribute = commercetools.Sdk.Domain.Products.Attributes.Attribute;

namespace commercetools.Sdk.HttpApi.Tests
{
    public class ProductFixture : ClientFixture, IDisposable
    {
        private readonly ProductTypeFixture productTypeFixture;
        public CategoryFixture CategoryFixture { get; }
        public List<Product> ProductsToDelete { get; }

        public ProductFixture()
        {
            this.ProductsToDelete = new List<Product>();
            this.productTypeFixture = new ProductTypeFixture();
            this.CategoryFixture = new CategoryFixture();
        }

        public void Dispose()
        {            
            IClient commerceToolsClient = this.GetService<IClient>();
            this.ProductsToDelete.Reverse();
            foreach (Product product in this.ProductsToDelete)
            {
                Product deletedType = commerceToolsClient.ExecuteAsync(new DeleteByIdCommand<Product>(new Guid(product.Id), product.Version)).Result;
            }
            this.productTypeFixture.Dispose();
            this.CategoryFixture.Dispose();
        }

        private Product Unpublish(Product product)
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>>();
            UnpublishUpdateAction unpublish = new UnpublishUpdateAction();
            updateActions.Add(unpublish);
            Product retrievedProduct = commerceToolsClient.ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions)).Result;
            return retrievedProduct;
        }

        public ProductDraft GetProductDraft(Category category)
        {
            ProductType productType = this.productTypeFixture.CreateProductType();
            this.productTypeFixture.ProductTypesToDelete.Add(productType);
            ProductDraft productDraft = new ProductDraft();
            productDraft.Name = new LocalizedString() { { "en", this.RandomString(4) } };
            productDraft.Slug = new LocalizedString() { { "en", this.RandomString(3) } };
            productDraft.ProductType = new ResourceIdentifier() { Id = productType.Id };
            ProductVariantDraft productVariantDraft = new ProductVariantDraft();
            productDraft.MasterVariant = productVariantDraft;
            List<Attribute> attributes = new List<Attribute>();
            productVariantDraft.Attributes = attributes;
            attributes.Add(new TextAttribute() { Name = "text-attribute-name", Value = this.RandomString(5)});
            attributes.Add(new LocalizedTextAttribute() { Name = "localized-text-attribute-name", Value = new LocalizedString() { { "en", this.RandomString(5) } } });
            attributes.Add(new EnumAttribute() { Name = "enum-attribute-name", Value = new PlainEnumValue() { Key = "enum-key-1" } });
            attributes.Add(new LocalizedEnumAttribute() { Name = "localized-enum-attribute-name", Value = new LocalizedEnumValue() { Key = "enum-key-1" } });
            attributes.Add(new BooleanAttribute() { Name = "boolean-attribute-name", Value = true });
            attributes.Add(new NumberAttribute() { Name = "number-attribute-name", Value = 10 });
            attributes.Add(new DateTimeAttribute() { Name = "date-time-attribute-name", Value = new DateTime(2018, 12, 10, 23, 43, 02) });
            attributes.Add(new DateAttribute() { Name = "date-attribute-name", Value = new DateTime(2018, 12, 10) });
            attributes.Add(new TimeAttribute() { Name = "time-attribute-name", Value = new TimeSpan(23, 43, 10) });
            attributes.Add(new MoneyAttribute(){ Name = "money-attribute-name", Value = new Money() { CentAmount = 4000, CurrencyCode = "EUR"} });
            attributes.Add(new ReferenceAttribute() { Name = "reference-attribute-name", Value = new Reference<Category>() { Id = category.Id, TypeId = ReferenceTypeId.Category }});
            SetTextAttribute setAttribute = new SetTextAttribute();
            AttributeSet<string> stringSet = new AttributeSet<string>() { "test1", "test2" };
            setAttribute.Value = stringSet;
            setAttribute.Name = "set-text-attribute-name";
            attributes.Add(setAttribute);
            productDraft.Categories = new List<ResourceIdentifier>
            {
                new ResourceIdentifier() { Id = category.Id }
            };
            return productDraft;
        }

        public Product CreateProduct()
        {
            Category category = this.CategoryFixture.CreateCategory();
            this.CategoryFixture.CategoriesToDelete.Add(category);
            return this.CreateProduct(this.GetProductDraft(category));
        }

        public ProductDraft GetProductDraft()
        {
            Category category = this.CategoryFixture.CreateCategory();
            this.CategoryFixture.CategoriesToDelete.Add(category);
            return this.GetProductDraft(category);
        }

        public Product CreateProduct(Category category)
        {
            return this.CreateProduct(this.GetProductDraft(category));
        }

        public Product CreateProduct(ProductDraft productDraft)
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            Product product = commerceToolsClient.ExecuteAsync(new CreateCommand<Product>(productDraft)).Result;
            return product;
        }
    }
}
