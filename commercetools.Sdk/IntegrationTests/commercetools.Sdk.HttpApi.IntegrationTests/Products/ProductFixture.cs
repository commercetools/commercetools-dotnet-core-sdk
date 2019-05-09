﻿using System;
using System.Collections.Generic;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.ProductDiscounts;
using commercetools.Sdk.Domain.Products.UpdateActions;
using commercetools.Sdk.Domain.States;
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

        public CategoryFixture CategoryFixture { get; }
        public List<Product> ProductsToDelete { get; }

        public ProductFixture(IMessageSink diagnosticMessageSink) : base(diagnosticMessageSink)
        {
            this.ProductsToDelete = new List<Product>();
            this.productTypeFixture = new ProductTypeFixture(diagnosticMessageSink);
            this.CategoryFixture = new CategoryFixture(diagnosticMessageSink);
            this.taxCategoryFixture = new TaxCategoryFixture(diagnosticMessageSink);
            this.typeFixture = new TypeFixture(diagnosticMessageSink);
            this.statesFixture = new StatesFixture(diagnosticMessageSink);
        }

        public void Dispose()
        {
            Product toBeDeleted = null;
            IClient commerceToolsClient = this.GetService<IClient>();
            this.ClearAllProductDiscounts();//Delete All Product discounts first if exists
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

        public ProductDraft GetProductDraft(Category category, ProductType productType, bool withVariants = false,
            bool publish = false, bool withImages = false, bool withAssets = false)
        {
            ProductDraft productDraft = new ProductDraft();
            productDraft.Key = TestingUtility.RandomString(10);
            productDraft.Name = new LocalizedString() {{"en", TestingUtility.RandomString(10)}};
            productDraft.Description = new LocalizedString() {{"en", TestingUtility.RandomString(20)}};
            productDraft.Slug = new LocalizedString() {{"en", TestingUtility.RandomString(10)}};
            productDraft.ProductType = new ResourceIdentifier<ProductType> {Key = productType.Key};
            ProductVariantDraft productMasterVariant =
                TestingUtility.GetRandomProductVariantDraft(category.Id, ReferenceTypeId.Category);
            productDraft.MasterVariant = productMasterVariant;

            productDraft.Categories = new List<IReference<Category>>
            {
                new ResourceIdentifier<Category> {Key = category.Key}
            };
            productDraft.Publish = publish; //if true, the product is published immediately

            if (withVariants) //then create variants for this product
            {
                productDraft.Variants = new List<ProductVariantDraft>();
                for (var i = 0; i < 3; i++) // add 3 variants
                {
                    var productVariantDraft =
                        TestingUtility.GetRandomProductVariantDraft(category.Id, ReferenceTypeId.Category);
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
        /// <param name="withImages">if true, this master product variant will have list of images.</param>
        /// <param name="withAssets">if true, this master product variant will have list of assets.</param>
        /// <returns></returns>
        public Product CreateProduct(bool withVariants = false, bool publish = false, bool withImages = false,
            bool withAssets = false)
        {
            Category category = this.CreateNewCategory();
            ProductType productType = this.CreateNewProductType();
            return this.CreateProduct(this.GetProductDraft(category, productType, withVariants, publish, withImages,
                withAssets));
        }

        public Product CreateProduct(Category category, ProductType productType, bool withVariants = false,
            bool publish = false, bool withImages = false, bool withAssets = false)
        {
            return this.CreateProduct(this.GetProductDraft(category, productType, withVariants, publish, withImages,
                withAssets));
        }

        public Product CreateProduct(ProductType productType, bool withVariants = false, bool publish = false,
            bool withImages = false, bool withAssets = false)
        {
            Category category = this.CreateNewCategory();
            return this.CreateProduct(this.GetProductDraft(category, productType, withVariants, publish, withImages,
                withAssets));
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

        public State CreateNewState(StateType stateType = StateType.ProductState,bool initial = true)
        {
            State state = this.statesFixture.CreateState(stateType, initial);
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

        private void ClearAllProductDiscounts()
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            //Query All Product Discounts first
            QueryCommand<ProductDiscount> queryCommand = new QueryCommand<ProductDiscount>();
            PagedQueryResult<ProductDiscount> returnedSet = commerceToolsClient.ExecuteAsync(queryCommand).Result;
            foreach (var productDiscount in returnedSet.Results)
            {
                var deletedType=commerceToolsClient
                    .ExecuteAsync(
                        new DeleteByIdCommand<ProductDiscount>(new Guid(productDiscount.Id), productDiscount.Version)).Result;
            }
        }
    }
}
