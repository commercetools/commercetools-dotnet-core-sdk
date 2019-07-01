﻿using System;
using System.Collections.Generic;
 using System.Threading.Tasks;
 using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.Domain.Common;
 using commercetools.Sdk.Domain.Orders;
 using commercetools.Sdk.Domain.ProductDiscounts;
using commercetools.Sdk.Domain.Products.UpdateActions;
using commercetools.Sdk.Domain.States;
 using commercetools.Sdk.HttpApi.Domain.Exceptions;
 using commercetools.Sdk.HttpApi.IntegrationTests.States;
using commercetools.Sdk.HttpApi.IntegrationTests.TaxCategories;
using Xunit;
using Xunit.Abstractions;
using Type = commercetools.Sdk.Domain.Type;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace commercetools.Sdk.HttpApi.IntegrationTests.Products
{
    public class ProductFixture : ClientFixture, IDisposable
    {
        private readonly ProductTypeFixture productTypeFixture;
        private readonly TaxCategoryFixture taxCategoryFixture;
        private readonly TypeFixture typeFixture;
        private readonly StatesFixture statesFixture;
        private readonly CategoryFixture categoryFixture;

        public List<Product> ProductsToDelete { get; }

        public ProductFixture(ServiceProviderFixture serviceProviderFixture) : base(serviceProviderFixture)
        {
            this.ProductsToDelete = new List<Product>();
            this.productTypeFixture = new ProductTypeFixture(serviceProviderFixture);
            this.categoryFixture = new CategoryFixture(serviceProviderFixture);
            this.taxCategoryFixture = new TaxCategoryFixture(serviceProviderFixture);
            this.typeFixture = new TypeFixture(serviceProviderFixture);
            this.statesFixture = new StatesFixture(serviceProviderFixture);
        }

        public void Dispose()
        {
            this.ProductsToDelete.Reverse();
            foreach (Product product in this.ProductsToDelete)
            {
                this.DeleteProduct(product);
            }

            this.productTypeFixture.Dispose();
            this.categoryFixture.Dispose();
            this.taxCategoryFixture.Dispose();
            this.typeFixture.Dispose();
            this.statesFixture.Dispose();
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

        public ProductDraft GetProductDraft(ProductType productType, Category category = null, bool withVariants = false,
            bool publish = false, bool withImages = false, bool withAssets = false, IReference<TaxCategory> taxCategoryReference = null, LocalizedString productName = null)
        {
            ProductDraft productDraft = new ProductDraft();
            productDraft.Key = TestingUtility.RandomString(10);
            productDraft.Name = productName?? new LocalizedString() {{"en", TestingUtility.RandomString(10)}};
            productDraft.Description = new LocalizedString() {{"en", TestingUtility.RandomString(20)}};
            productDraft.Slug = new LocalizedString() {{"en", TestingUtility.RandomString(10)}};
            productDraft.ProductType = new ResourceIdentifier<ProductType> {Key = productType.Key};

            var productMasterVariant =
                TestingUtility.GetRandomProductVariantDraft(productType.Id, ReferenceTypeId.ProductType);
            productDraft.MasterVariant = productMasterVariant;

            if (category != null)
            {
                productDraft.Categories = new List<IReference<Category>>
                {
                    new ResourceIdentifier<Category> {Key = category.Key}
                };
            }

            productDraft.Publish = publish; //if true, the product is published immediately

            if (withVariants) //then create variants for this product
            {
                productDraft.Variants = new List<ProductVariantDraft>();
                for (var i = 0; i < 3; i++) // add 3 variants
                {
                    var productVariantDraft = TestingUtility.GetRandomProductVariantDraft(productType.Id, ReferenceTypeId.ProductType);
                    productDraft.Variants.Add(productVariantDraft);
                }
            }

            if (withImages)
            {
                productDraft.MasterVariant.Images = TestingUtility.GetListOfImages(3);
            }

            if (withAssets)
            {
                productDraft.MasterVariant.Assets = TestingUtility.GetListOfAssetsDrafts(3);
            }

            //Add taxCategory to product (if taxCategoryReference is null, then create new TaxCategory)
            var taxCategory = taxCategoryReference?? CreateNewTaxCategory().ToReference();
            productDraft.TaxCategory = taxCategory;

            return productDraft;
        }

        /// <summary>
        /// Create a Product
        /// </summary>
        /// <param name="withVariants">if true then it will create product with product variants, else it will create product with empty variants</param>
        /// <param name="publish">if true, this product is published immediately.</param>
        /// <param name="withImages">if true, this master product variant will have list of images.</param>
        /// <param name="withAssets">if true, this master product variant will have list of assets.</param>
        /// <param name="taxCategoryReference">if not null, then product will created with this taxCategory</param>
        /// <param name="productName">if not null, then product will created with this localized name</param>
        /// <param name="productCategory">if not null, then product will created with this productCategory</param>
        /// <param name="productType">if not null, then product will created with this productType</param>
        /// <returns></returns>
        public Product CreateProduct(bool withVariants = false, bool publish = false, bool withImages = false,
            bool withAssets = false, IReference<TaxCategory> taxCategoryReference = null, LocalizedString productName = null, Category productCategory = null, ProductType productType = null)
        {
            Category category = productCategory;
            ProductType pType = productType?? this.CreateNewProductType();
            return this.CreateProduct(this.GetProductDraft(pType, category, withVariants, publish, withImages,
                withAssets, taxCategoryReference, productName));
        }

        public Product CreateProduct(Category category, ProductType productType, bool withVariants = false,
            bool publish = false, bool withImages = false, bool withAssets = false)
        {
            return this.CreateProduct(this.GetProductDraft(productType, category, withVariants, publish, withImages,
                withAssets));
        }

        public Product CreateProduct(ProductType productType, bool withVariants = false, bool publish = false,
            bool withImages = false, bool withAssets = false)
        {
            Category category = this.CreateNewCategory();
            return this.CreateProduct(this.GetProductDraft(productType, category, withVariants, publish, withImages,
                withAssets));
        }

        public ProductDraft GetProductDraft()
        {
            Category category = this.CreateNewCategory();
            ProductType productType = this.CreateNewProductType();
            return this.GetProductDraft(productType, category);
        }

        public Product CreateProduct(ProductDraft productDraft)
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            Product product = commerceToolsClient.ExecuteAsync(new CreateCommand<Product>(productDraft)).Result;
            return product;
        }

        public Category CreateNewCategory()
        {
            Category category = this.categoryFixture.CreateCategory();
            this.categoryFixture.CategoriesToDelete.Add(category);
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

        public State CreateNewState(StateType stateType = StateType.ProductState,bool initial = true)
        {
            string stateKey = $"Key-{TestingUtility.RandomInt()}";
            State state = this.statesFixture.CreateState(stateKey, stateType, initial);
            this.statesFixture.StatesToDelete.Add(state);
            return state;
        }


        /// <summary>
        /// Get Product Discount Draft with external product discount value and can be used for all products
        /// </summary>
        /// <returns></returns>
        private ProductDiscountDraft GetProductDiscountDraft()
        {
            string predicate = "1 = 1";//can be used for all products
            ProductDiscountDraft productDiscountDraft = new ProductDiscountDraft();
            productDiscountDraft.Name = new LocalizedString() {{"en", TestingUtility.RandomString(10)}};
            productDiscountDraft.Value = new ExternalProductDiscountValue();
            productDiscountDraft.Predicate = predicate;
            productDiscountDraft.SortOrder = TestingUtility.RandomSortOrder();
            productDiscountDraft.IsActive = true;

            return productDiscountDraft;
        }

        public ProductDiscount CreateProductDiscount()
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            var productDiscountDraft = GetProductDiscountDraft();
            ProductDiscount productDiscount = commerceToolsClient
                .ExecuteAsync(new CreateCommand<ProductDiscount>(productDiscountDraft)).Result;
            return productDiscount;
        }

        public ProductVariantImportDraft GetProductVariantImportDraftBySku(string sku = null)
        {
            var productVariantImportDraft = new ProductVariantImportDraft(sku);
            return productVariantImportDraft;
        }
        public ProductVariantImportDraft GetProductVariantImportDraftByProductId(string productId, int variantId)
        {
            var productVariantImportDraft = new ProductVariantImportDraft(productId, variantId);
            return productVariantImportDraft;
        }

        public LineItemImportDraft GetLineItemImportDraftByProductId(string productId, int variantId = 1)
        {
            var productVariantImportDraft = GetProductVariantImportDraftByProductId(productId, variantId);
            var lineItemImportDraft = this.GetLineItemImportDraft();
            lineItemImportDraft.Variant = productVariantImportDraft;
            lineItemImportDraft.ProductId = productId;
            return lineItemImportDraft;
        }

        public LineItemImportDraft GetLineItemImportDraftBySku(string sku)
        {
            var productVariantImportDraft = GetProductVariantImportDraftBySku(sku);
            var lineItemImportDraft = this.GetLineItemImportDraft();
            lineItemImportDraft.Variant = productVariantImportDraft;
            return lineItemImportDraft;
        }

        private LineItemImportDraft GetLineItemImportDraft()
        {
            var lineItemImportDraft = new LineItemImportDraft
            {
                Quantity = 1,
                Price = TestingUtility.GetRandomPrice(),
                Name = new LocalizedString() {{"en", TestingUtility.RandomString(10)}}
            };
            return lineItemImportDraft;
        }

        /// <summary>
        /// Delete Product
        /// </summary>
        /// <param name="product"></param>
        /// <returns>return the deleted product</returns>
        private Product DeleteProduct(Product product)
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            var toBeDeleted = product;
            if (product.MasterData.Published) // unpublish it before delete
            {
                toBeDeleted = Unpublish(product);
            }

            return this.TryDeleteResource(toBeDeleted).Result;
        }

        public Product GetProductById(string id)
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            return commerceToolsClient.ExecuteAsync(new GetByIdCommand<Product>(id)).Result;
        }
    }
}
