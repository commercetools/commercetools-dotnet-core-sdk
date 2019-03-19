using System;
using System.Collections.Generic;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.Domain.Orders;
using commercetools.Sdk.Domain.Products;
using commercetools.Sdk.Domain.Products.Attributes;
using Xunit.Abstractions;
using Attribute = commercetools.Sdk.Domain.Products.Attributes.Attribute;

namespace commercetools.Sdk.HttpApi.IntegrationTests
{
    public class ProductFixture : ClientFixture, IDisposable
    {
        private readonly ProductTypeFixture productTypeFixture;
        public CategoryFixture CategoryFixture { get; }
        public List<Product> ProductsToDelete { get; }

        public ProductFixture() : base()
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
                Product deletedType = commerceToolsClient
                    .ExecuteAsync(new DeleteByIdCommand<Product>(new Guid(product.Id), product.Version)).Result;
            }

            this.productTypeFixture.Dispose();
            this.CategoryFixture.Dispose();
        }

        public Product Publish(Product product, PublishScope scope = PublishScope.All)
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>>();
            PublishUpdateAction publishUpdateAction = new PublishUpdateAction()
            {
                Scope = scope
            };
            updateActions.Add(publishUpdateAction);
            Product retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions))
                .Result;
            return retrievedProduct;
        }

        public Product Unpublish(Product product)
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>>();
            UnpublishUpdateAction unpublish = new UnpublishUpdateAction();
            updateActions.Add(unpublish);
            Product retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions))
                .Result;
            return retrievedProduct;
        }

        public ProductDraft GetProductDraft(Category category, ProductType productType, bool withVariants = false)
        {
            ProductDraft productDraft = new ProductDraft();
            productDraft.Name = new LocalizedString() {{"en", this.RandomString(4)}};
            productDraft.Key = this.RandomString(3);
            productDraft.Slug = new LocalizedString() {{"en", this.RandomString(3)}};
            productDraft.ProductType = new ResourceIdentifier() {Id = productType.Id};
            ProductVariantDraft productMasterVariant =
                this.GetRandomProductVariantDraft(category.Id, ReferenceTypeId.Category);
            productDraft.MasterVariant = productMasterVariant;
            productDraft.Categories = new List<ResourceIdentifier>
            {
                new ResourceIdentifier() {Id = category.Id}
            };

            if (withVariants) //then create variants for this product
            {
                productDraft.Variants = new List<ProductVariantDraft>();
                var productVariantDraft = this.GetRandomProductVariantDraft(category.Id, ReferenceTypeId.Category);
                productDraft.Variants.Add(productVariantDraft);
            }

            return productDraft;
        }

        /// <summary>
        /// Create a Product
        /// </summary>
        /// <param name="withVariants">if true then it will create product with product variants, else it will create product with empty variants</param>
        /// <returns></returns>
        public Product CreateProduct(bool withVariants = false)
        {
            Category category = this.CreateNewCategory();
            ProductType productType = this.CreateNewProductType();
            return this.CreateProduct(this.GetProductDraft(category, productType, withVariants));
        }

        public Product CreateProduct(Category category, ProductType productType, bool withVariants = false)
        {
            return this.CreateProduct(this.GetProductDraft(category, productType, withVariants));
        }
        public Product CreateProduct(ProductType productType, bool withVariants = false)
        {
            Category category = this.CreateNewCategory();
            return this.CreateProduct(this.GetProductDraft(category, productType, withVariants));
        }

        /// <summary>
        /// Create product and publish it
        /// </summary>
        /// <param name="withVariants"></param>
        /// <returns></returns>
        public Product CreateProductAndPublishIt(bool withVariants = false)
        {
            var product = this.CreateProduct(withVariants);
            product = this.Publish(product);
            return product;
        }

        public Product CreateProductAndPublishIt(ProductType productType, bool withVariants = false)
        {
            Category category = this.CreateNewCategory();
            var product = this.CreateProduct(category, productType, withVariants);
            product = this.Publish(product);
            return product;
        }

        public ProductDraft GetProductDraft()
        {
            Category category = this.CreateNewCategory();
            ProductType productType = this.CreateNewProductType();
            return this.GetProductDraft(category,productType);
        }

        public Product CreateProduct(ProductDraft productDraft)
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            Product product = commerceToolsClient.ExecuteAsync(new CreateCommand<Product>(productDraft)).Result;
            return product;
        }

        /// <summary>
        /// Get Random Product Variant Draft with attributes
        /// </summary>
        /// <param name="referenceAttributeId"></param>
        /// <param name="referenceTypeId"></param>
        /// <returns></returns>
        public ProductVariantDraft GetRandomProductVariantDraft(string referenceAttributeId = "",
            ReferenceTypeId? referenceTypeId = null)
        {
            var price = GetRandomPriceDraft();
            var productVariantDraft = new ProductVariantDraft()
            {
                Key = this.RandomString(5),
                Sku = this.RandomString(5),
                Prices = new List<PriceDraft>(){price},
                Attributes = GetListOfRandomAttributes(referenceAttributeId, referenceTypeId)
            };
            return productVariantDraft;
        }

        /// <summary>
        /// Get Random Price Draft
        /// </summary>
        /// <returns></returns>
        internal PriceDraft GetRandomPriceDraft()
        {
            var money = new Money()
            {
                CentAmount = this.RandomInt(1000, 5000),
                CurrencyCode = "EUR"
            };
            var priceDraft = new PriceDraft()
            {
                Value = money,
                Country = "DE",
                ValidFrom = DateTime.Today.AddMonths(this.RandomInt(-5, -1)),
                ValidUntil = DateTime.Today.AddMonths(this.RandomInt(1, 5))
            };
            return priceDraft;
        }

        //TODO Add utility class for creating random strings,numbers,years,dates,...etc for testing
        /// <summary>
        /// Get list of Random attributes with reference attribute if passed
        /// </summary>
        /// <param name="referenceAttributeId"></param>
        /// <param name="referenceTypeId"></param>
        /// <returns></returns>
        internal List<Attribute> GetListOfRandomAttributes(string referenceAttributeId = "",
            ReferenceTypeId? referenceTypeId = null)
        {
            List<Attribute> attributes = new List<Attribute>();
            attributes.Add(new TextAttribute() {Name = "text-attribute-name", Value = this.RandomString(5)});
            attributes.Add(new LocalizedTextAttribute()
                {Name = "localized-text-attribute-name", Value = new LocalizedString() {{"en", this.RandomString(5)}}});
            attributes.Add(new EnumAttribute()
                {Name = "enum-attribute-name", Value = new PlainEnumValue() {Key = "enum-key-1"}});
            attributes.Add(new LocalizedEnumAttribute()
                {Name = "localized-enum-attribute-name", Value = new LocalizedEnumValue() {Key = "enum-key-1"}});
            attributes.Add(new BooleanAttribute() {Name = "boolean-attribute-name", Value = true});
            attributes.Add(new NumberAttribute() {Name = "number-attribute-name", Value = 10});
            attributes.Add(new DateTimeAttribute()
                {Name = "date-time-attribute-name", Value = new DateTime(2018, 12, 10, 23, 43, 02)});
            attributes.Add(new DateAttribute() {Name = "date-attribute-name", Value = new DateTime(2018, 12, 10)});
            attributes.Add(new TimeAttribute() {Name = "time-attribute-name", Value = new TimeSpan(23, 43, 10)});
            attributes.Add(new MoneyAttribute()
                {Name = "money-attribute-name", Value = new Money() {CentAmount = 4000, CurrencyCode = "EUR"}});
            if (!string.IsNullOrEmpty(referenceAttributeId) && referenceTypeId != null)
            {
                attributes.Add(new ReferenceAttribute()
                {
                    Name = "reference-attribute-name",
                    Value = new Reference<Category>() {Id = referenceAttributeId, TypeId = referenceTypeId.Value}
                });
            }

            SetTextAttribute setAttribute = new SetTextAttribute();
            AttributeSet<string> stringSet = new AttributeSet<string>() {"test1", "test2"};
            setAttribute.Value = stringSet;
            setAttribute.Name = "set-text-attribute-name";
            attributes.Add(setAttribute);
            return attributes;
        }

        public Category CreateNewCategory()
        {
            Category category = this.CategoryFixture.CreateCategory();
            this.CategoryFixture.CategoriesToDelete.Add(category);
            return category;
        }

        public ProductType CreateNewProductType()
        {
            ProductType productType = this.productTypeFixture.CreateProductType();
            this.productTypeFixture.ProductTypesToDelete.Add(productType);
            return productType;
        }
    }
}
