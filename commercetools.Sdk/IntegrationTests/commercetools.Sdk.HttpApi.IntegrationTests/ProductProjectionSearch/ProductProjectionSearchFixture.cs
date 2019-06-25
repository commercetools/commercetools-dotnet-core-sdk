using System;
using System.Collections.Generic;
using System.Linq;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.Domain.Channels;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Orders;
using commercetools.Sdk.Domain.ProductDiscounts;
using commercetools.Sdk.Domain.Reviews;
using commercetools.Sdk.Domain.States;
using commercetools.Sdk.HttpApi.IntegrationTests.Channels;
using commercetools.Sdk.HttpApi.IntegrationTests.Inventory;
using commercetools.Sdk.HttpApi.IntegrationTests.ProductDiscounts;
using commercetools.Sdk.HttpApi.IntegrationTests.Products;
using commercetools.Sdk.HttpApi.IntegrationTests.Project;
using commercetools.Sdk.HttpApi.IntegrationTests.Reviews;
using commercetools.Sdk.HttpApi.IntegrationTests.States;
using commercetools.Sdk.HttpApi.IntegrationTests.TaxCategories;
using Xunit.Abstractions;
using Attribute = commercetools.Sdk.Domain.Products.Attributes.Attribute;

namespace commercetools.Sdk.HttpApi.IntegrationTests.ProductProjectionSearch
{
    public class ProductProjectionSearchFixture : ClientFixture, IDisposable
    {
        private readonly ProductFixture productFixture;
        private readonly ProjectFixture projectFixture;
        private readonly CategoryFixture categoryFixture;
        private readonly ProductTypeFixture productTypeFixture;
        private readonly TaxCategoryFixture taxCategoryFixture;
        private readonly StatesFixture stateFixture;
        private readonly InventoryFixture inventoryFixture;
        private readonly ChannelFixture channelFixture;
        private readonly ProductDiscountsFixture productDiscountsFixture;
        private readonly ReviewFixture reviewFixture;

        public ProductProjectionSearchFixture(ServiceProviderFixture serviceProviderFixture) : base(
            serviceProviderFixture)
        {
            this.productFixture = new ProductFixture(serviceProviderFixture);
            this.projectFixture = new ProjectFixture(serviceProviderFixture);
            this.categoryFixture = new CategoryFixture(serviceProviderFixture);
            this.productTypeFixture = new ProductTypeFixture(serviceProviderFixture);
            this.taxCategoryFixture = new TaxCategoryFixture(serviceProviderFixture);
            this.stateFixture = new StatesFixture(serviceProviderFixture);
            this.inventoryFixture = new InventoryFixture(serviceProviderFixture);
            this.channelFixture = new ChannelFixture(serviceProviderFixture);
            this.productDiscountsFixture = new ProductDiscountsFixture(serviceProviderFixture);
            this.reviewFixture = new ReviewFixture(serviceProviderFixture);
        }

        public void Dispose()
        {
            this.productDiscountsFixture.Dispose();
            this.reviewFixture.Dispose();
            this.productFixture.Dispose();
            this.inventoryFixture.Dispose();
            this.categoryFixture.Dispose();
            this.productTypeFixture.Dispose();
            this.taxCategoryFixture.Dispose();
            this.stateFixture.Dispose();
            this.channelFixture.Dispose();
        }

        public Category CreateNewCategory()
        {
            Category category = this.categoryFixture.CreateCategory();
            this.categoryFixture.CategoriesToDelete.Add(category);
            return category;
        }

        public Channel CreateNewChannel(ChannelRole role)
        {
            var channel = this.channelFixture.CreateChannel(role);
            this.channelFixture.ChannelsToDelete.Add(channel);
            return channel;
        }

        public ProductType CreateNewProductType()
        {
            var productType = this.productTypeFixture.CreateProductType();
            this.productTypeFixture.ProductTypesToDelete.Add(productType);
            return productType;
        }

        public InventoryEntry CreateInventoryEntry(InventoryEntryDraft draft)
        {
            var inventoryEntry = this.inventoryFixture.CreateInventoryEntry(draft);
            this.inventoryFixture.InventoryEntries.Add(inventoryEntry);
            return inventoryEntry;
        }

        public TaxCategory CreateNewTaxCategory()
        {
            var taxCategory = this.taxCategoryFixture.CreateTaxCategory();
            this.taxCategoryFixture.TaxCategoriesToDelete.Add(taxCategory);
            return taxCategory;
        }

        public State CreateNewState()
        {
            var key = TestingUtility.RandomString(10);
            var state = this.stateFixture.CreateState(key);
            this.stateFixture.StatesToDelete.Add(state);
            return state;
        }

        /// <summary>
        /// Create a product with localized product name
        /// </summary>
        /// <param name="productName"></param>
        /// <returns></returns>
        public Product CreateProductWithLocalizedName(LocalizedString productName)
        {
            var product = this.productFixture.CreateProduct(productName: productName);
            this.productFixture.ProductsToDelete.Add(product);
            return product;
        }

        public Product CreateProduct(bool withVariants = false, bool publish = false, bool withImages = false,
            bool withAssets = false, IReference<TaxCategory> taxCategoryReference = null,
            LocalizedString productName = null, Category productCategory = null, ProductType productType = null)
        {
            var product = this.productFixture.CreateProduct(withVariants, publish, withImages, withAssets,
                taxCategoryReference, productName, productCategory, productType);
            this.productFixture.ProductsToDelete.Add(product);
            return product;
        }

        public Product CreateCustomizedProduct(string sku = null, string productKey = null, string productVariantKey = null, TaxCategory taxCategory = null, List<PriceDraft> prices = null, State state = null, List<Attribute> attributes = null, ProductVariantAvailability productVariantAvailability = null, Channel supplyChannel = null, Dictionary<string, List<SearchKeywords>> searchKeywords = null)
        {
            var productDraft = this.productFixture.GetProductDraft();
            productDraft.MasterVariant.Sku = sku;
            productDraft.Key = productKey;
            productDraft.MasterVariant.Key = productVariantKey;
            productDraft.MasterVariant.Prices = prices;
            productDraft.MasterVariant.Attributes = attributes;
            productDraft.TaxCategory = taxCategory?.ToReference();
            productDraft.State = state?.ToReference();
            productDraft.SearchKeywords = searchKeywords;

            var product = this.productFixture.CreateProduct(productDraft);

            if (productVariantAvailability != null)
            {
                // then add inventory it for this product, and get product again
                var inventoryDraft = new InventoryEntryDraft
                {
                    Sku = product.MasterData.Staged.MasterVariant.Sku,
                    QuantityOnStock = productVariantAvailability.AvailableQuantity,
                    SupplyChannel = supplyChannel?.ToReference(),
                    RestockableInDays = productVariantAvailability.RestockableInDays
                };
                this.CreateInventoryEntry(inventoryDraft);
                product = this.productFixture.GetProductById(product.Id);
            }

            this.productFixture.ProductsToDelete.Add(product);
            return product;
        }

        public ProductDiscount CreateProductDiscountOfAbsoluteValue(string productId, int centAmount)
        {
            var discountDraft = this.productDiscountsFixture.GetProductDiscountDraft(productId, centAmount);
            var productDiscount = this.productDiscountsFixture.CreateProductDiscount(discountDraft);
            this.productDiscountsFixture.ProductDiscountsToDelete.Add(productDiscount);
            return productDiscount;
        }

        public Review CreateProductReview(string productKey, int rating)
        {
            var reviewDraft = new ReviewDraft
            {
                Rating = rating,
                Target = new ResourceIdentifier<Product> { Key = productKey }
            };
            var review = this.reviewFixture.CreateReview(reviewDraft);
            this.reviewFixture.ReviewsToDelete.Add(review);
            return review;
        }

        public Sdk.Domain.Project.Project ChangeProjectLanguages(List<string> languages)
        {
            var project = this.projectFixture.ChangeProjectLanguages(languages);
            return project;
        }

        /// <summary>
        /// Create Parent and child categories
        /// </summary>
        /// <returns>return list of parent and child categories</returns>
        public List<Category> SetupCategories()
        {
            var allCategories = new List<Category>();
            var rootCategories = new List<Category>();
            var secondLevelCategories = new List<Category>();

            var root = new List<string> {"A", "B", "C"};
            var numberOfChild = 2;
            var rand = TestingUtility.RandomInt();

            foreach (var rootCat in root)
            {
                var rootCategory = this.CreateParentCategory(rootCat);
                rootCategories.Add(rootCategory);

                //create child categories
                for (var i = 1; i <= numberOfChild; i++)
                {
                    var childCategory = this.CreateChildCategory(rootCategory, i);
                    secondLevelCategories.Add(childCategory);
                }
            }

            allCategories.AddRange(rootCategories);
            allCategories.AddRange(secondLevelCategories);
            this.categoryFixture.CategoriesToDelete.AddRange(allCategories);
            return allCategories;
        }

        private Category CreateParentCategory(string categoryLabel)
        {
            var rand = TestingUtility.RandomInt();
            var draft = this.categoryFixture.GetCategoryDraft(
                new LocalizedString {{"en", $"name {categoryLabel}-{rand}"}},
                new LocalizedString {{"en", $"slug-{categoryLabel}-{rand}"}},
                $"key-{categoryLabel}-{rand}",
                externalId: categoryLabel);
            var rootCategory = this.categoryFixture.CreateCategory(draft);
            return rootCategory;
        }
        private Category CreateChildCategory(Category rootCategory, int index)
        {
            var rand = TestingUtility.RandomInt();

            var id = $"{rootCategory.ExternalId}-{index}"; //A-1
            var childDraft = this.categoryFixture.GetCategoryDraft(
                new LocalizedString {{"en", $"name {id}-{rand}"}},
                new LocalizedString {{"en", $"slug-{id}-{rand}"}},
                $"key-{id}-{rand}",
                externalId: $"{rootCategory.ExternalId}{index}",
                parent: new ResourceIdentifier<Category>
                {
                    Key = rootCategory.Key
                });
            var childCategory = this.categoryFixture.CreateCategory(childDraft);
            return childCategory;
        }


        public void AssertEventually(Action runnableBlock, int maxWaitTimeSecond = 60, int waitBeforeRetryMilliseconds = 100)
        {
            var maxWaitTime = TimeSpan.FromSeconds(maxWaitTimeSecond);
            var waitBeforeRetry = TimeSpan.FromMilliseconds(waitBeforeRetryMilliseconds);
            this.AssertEventually(maxWaitTime, waitBeforeRetry, runnableBlock);
        }
    }
}
