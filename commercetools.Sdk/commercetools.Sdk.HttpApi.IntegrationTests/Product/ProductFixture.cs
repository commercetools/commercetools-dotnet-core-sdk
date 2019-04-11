using System;
using System.Collections.Generic;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.Domain.Orders;
using commercetools.Sdk.Domain.ProductDiscounts;
using commercetools.Sdk.Domain.Products;
using commercetools.Sdk.Domain.Products.Attributes;
using commercetools.Sdk.HttpApi.IntegrationTests.ProductDiscounts;
using commercetools.Sdk.HttpApi.IntegrationTests.TaxCategories;
using Xunit.Abstractions;
using Attribute = commercetools.Sdk.Domain.Products.Attributes.Attribute;
using Type = commercetools.Sdk.Domain.Type;

namespace commercetools.Sdk.HttpApi.IntegrationTests
{
    public class ProductFixture : ClientFixture, IDisposable
    {
        private readonly ProductTypeFixture productTypeFixture;
        private readonly TaxCategoryFixture taxCategoryFixture;
        private readonly TypeFixture typeFixture;
        private readonly ProductDiscountsFixture productDiscountsFixture;

        public CategoryFixture CategoryFixture { get; }
        public List<Product> ProductsToDelete { get; }

        public ProductFixture() : base()
        {
            this.ProductsToDelete = new List<Product>();
            this.productTypeFixture = new ProductTypeFixture();
            this.CategoryFixture = new CategoryFixture();
            this.taxCategoryFixture = new TaxCategoryFixture();
            this.typeFixture = new TypeFixture();
            //this.productDiscountsFixture = new ProductDiscountsFixture();
        }

        public void Dispose()
        {
            Product toBeDeleted = null;
            IClient commerceToolsClient = this.GetService<IClient>();
            this.ProductsToDelete.Reverse();
            foreach (Product product in this.ProductsToDelete)
            {
                toBeDeleted = product;
                if (product.MasterData.Published) // unpublish it before delete
                {
                    toBeDeleted = Unpublish(product);
                }

                Product deletedType = commerceToolsClient
                    .ExecuteAsync(new DeleteByIdCommand<Product>(new Guid(toBeDeleted.Id), toBeDeleted.Version)).Result;
            }

            this.productTypeFixture.Dispose();
            this.CategoryFixture.Dispose();
            this.taxCategoryFixture.Dispose();
            this.typeFixture.Dispose();
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

        public ProductDraft GetProductDraft(Category category, ProductType productType, bool withVariants = false,
            bool publish = false)
        {
            ProductDraft productDraft = new ProductDraft();
            productDraft.Name = new LocalizedString() {{"en", this.RandomString(10)}};
            productDraft.Key = this.RandomString(10);
            productDraft.Slug = new LocalizedString() {{"en", this.RandomString(10)}};
            productDraft.ProductType = new ResourceIdentifier<ProductType> {Key = productType.Key};
            ProductVariantDraft productMasterVariant =
                TestingUtility.GetRandomProductVariantDraft(category.Id, ReferenceTypeId.Category);
            productDraft.MasterVariant = productMasterVariant;

            productDraft.Categories = new List<IReferenceable<Category>>
            {
                new ResourceIdentifier<Category> {Key = category.Key}
            };
            productDraft.Publish = publish; //if true, the product is published immediately

            if (withVariants) //then create variants for this product
            {
                productDraft.Variants = new List<ProductVariantDraft>();
                for (var i = 0; i < 3; i++) // add 3 variants
                {
                    var productVariantDraft = TestingUtility.GetRandomProductVariantDraft(category.Id, ReferenceTypeId.Category);
                    productDraft.Variants.Add(productVariantDraft);
                }
            }

            //Add taxCategory to product
            var taxCategory = CreateNewTaxCategory();
            productDraft.TaxCategory = new Reference<TaxCategory>()
            {
                Id = taxCategory.Id
            };

            return productDraft;
        }

        /// <summary>
        /// Create a Product
        /// </summary>
        /// <param name="withVariants">if true then it will create product with product variants, else it will create product with empty variants</param>
        /// <param name="publish">if true, this product is published immediately.</param>
        /// <returns></returns>
        public Product CreateProduct(bool withVariants = false, bool publish = false)
        {
            Category category = this.CreateNewCategory();
            ProductType productType = this.CreateNewProductType();
            return this.CreateProduct(this.GetProductDraft(category, productType, withVariants, publish));
        }

        public Product CreateProduct(Category category, ProductType productType, bool withVariants = false,
            bool publish = false)
        {
            return this.CreateProduct(this.GetProductDraft(category, productType, withVariants, publish));
        }

        public Product CreateProduct(ProductType productType, bool withVariants = false, bool publish = false)
        {
            Category category = this.CreateNewCategory();
            return this.CreateProduct(this.GetProductDraft(category, productType, withVariants, publish));
        }

        public ProductDraft GetProductDraft()
        {
            Category category = this.CreateNewCategory();
            ProductType productType = this.CreateNewProductType();
            return this.GetProductDraft(category, productType);
        }

        public Product CreateProduct(ProductDraft productDraft)
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            Product product = commerceToolsClient.ExecuteAsync(new CreateCommand<Product>(productDraft)).Result;
            return product;
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

        public TaxCategory CreateNewTaxCategory()
        {
            var taxCategory = this.taxCategoryFixture.CreateTaxCategory();
            this.taxCategoryFixture.TaxCategoriesToDelete.Add(taxCategory);
            return taxCategory;
        }

        public Type CreateCustomType()
        {
            Type customType = this.typeFixture.CreateType();
            this.typeFixture.TypesToDelete.Add(customType);
            return customType;
        }
        public Fields CreateNewFields()
        {
            Fields fields = this.typeFixture.CreateNewFields();
            return fields;
        }
//        public ProductDiscount CreateProductDiscount()
//        {
//            var productDiscount = this.productDiscountsFixture.CreateProductDiscount();
//            this.productDiscountsFixture.ProductDiscountsToDelete.Add(productDiscount);
//            return productDiscount;
//        }
    }
}
