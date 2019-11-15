using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.Domain.Channels;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Orders;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.Domain.ProductDiscounts;
using commercetools.Sdk.Domain.Products.Attributes;
using commercetools.Sdk.Domain.Reviews;
using commercetools.Sdk.Domain.States;
using commercetools.Sdk.Domain.TaxCategories;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using commercetools.Sdk.HttpApi.IntegrationTests.Channels;
using commercetools.Sdk.HttpApi.IntegrationTests.Inventory;
using commercetools.Sdk.HttpApi.IntegrationTests.ProductDiscounts;
using commercetools.Sdk.HttpApi.IntegrationTests.Products;
using commercetools.Sdk.HttpApi.IntegrationTests.Reviews;
using commercetools.Sdk.HttpApi.IntegrationTests.States;
using commercetools.Sdk.HttpApi.IntegrationTests.TaxCategories;
using Castle.Components.DictionaryAdapter;
using commercetools.Sdk.Domain.InventoryEntries;
using commercetools.Sdk.Domain.Projects;
using commercetools.Sdk.HttpApi.IntegrationTests.Projects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;
using Attribute = commercetools.Sdk.Domain.Products.Attributes.Attribute;
using ReferenceAttribute = commercetools.Sdk.Domain.Products.Attributes.ReferenceAttribute;

namespace commercetools.Sdk.HttpApi.IntegrationTests.ProductProjectionSearch
{
    public class ProductProjectionSearchFixture : IDisposable
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
        private readonly int randomInt;

        private readonly ServiceProviderFixture serviceProviderFixture;
        public List<Product> AvailableProducts { get; set; }
        public List<Category> AvailableCategories { get; set; }
        public List<Channel> AvailableChannels { get; set; }
        public List<ProductType> AvailableProductTypes { get; set; }

        public Guid TestId => this.serviceProviderFixture.TestId;

        public ProductProjectionSearchFixture(IMessageSink diagnosticMessageSink)
        {
            //create serviceProviderFixture Instance
            this.serviceProviderFixture = new ServiceProviderFixture(diagnosticMessageSink);

            this.productFixture = new ProductFixture(this.serviceProviderFixture);
            this.projectFixture = new ProjectFixture(this.serviceProviderFixture);
            this.categoryFixture = new CategoryFixture(this.serviceProviderFixture);
            this.productTypeFixture = new ProductTypeFixture(this.serviceProviderFixture);
            this.taxCategoryFixture = new TaxCategoryFixture(this.serviceProviderFixture);
            this.stateFixture = new StatesFixture(this.serviceProviderFixture);
            this.inventoryFixture = new InventoryFixture(this.serviceProviderFixture);
            this.channelFixture = new ChannelFixture(this.serviceProviderFixture);
            this.productDiscountsFixture = new ProductDiscountsFixture(this.serviceProviderFixture);
            this.reviewFixture = new ReviewFixture(this.serviceProviderFixture);

            this.randomInt = TestingUtility.RandomInt();
            //Configure and Create all entities before running tests
            this.ConfigureAndCreateEntitiesBeforeRunningTests();
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

        public T GetService<T>()
        {
            return this.serviceProviderFixture.ServiceProvider.GetService<T>();
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

        private Product CreateCustomizedProduct(string sku = null, string productKey = null,
            string productVariantKey = null, TaxCategory taxCategory = null, List<PriceDraft> prices = null,
            State state = null, List<Attribute> attributes = null,
            ProductVariantAvailability productVariantAvailability = null, Channel supplyChannel = null,
            Dictionary<string, List<SearchKeywords>> searchKeywords = null, LocalizedString productName = null)
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
            productDraft.Name = productName?? new LocalizedString() {{"en", TestingUtility.RandomString(10)}};

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
                Target = new ResourceIdentifier<Product> {Key = productKey}
            };
            var review = this.reviewFixture.CreateReview(reviewDraft);
            this.reviewFixture.ReviewsToDelete.Add(review);
            return review;
        }

        public Project ChangeProjectLanguages(List<string> languages)
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


        public void AssertEventually(Action runnableBlock, int maxWaitTimeSecond = 300,
            int waitBeforeRetryMilliseconds = 100)
        {
            var maxWaitTime = TimeSpan.FromSeconds(maxWaitTimeSecond);
            var waitBeforeRetry = TimeSpan.FromMilliseconds(waitBeforeRetryMilliseconds);
            TestingUtility.AssertEventually(maxWaitTime, waitBeforeRetry, runnableBlock);
        }

        private void ConfigureAndCreateEntitiesBeforeRunningTests()
        {
            //update current project to support these languages
            var project = this.ChangeProjectLanguages(new List<string> {"en", "de"});
            Assert.Equal(2, project.Languages.Count);
            Assert.True(project.Languages.Contains("en") && project.Languages.Contains("de"));

            //Fill entities
            this.FillCategories();
            this.FillProducts();
            var allProductIds = AvailableProducts.Select(product => product.Id).ToArray();

            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(allProductIds));
            IClient commerceToolsClient = this.GetService<IClient>();

            //wait till elastic search index refreshed with created products
            AssertEventually(() =>
            {
                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Equal(allProductIds.Length, searchResults.Results.Count);
            });
        }

        private void FillCategories()
        {
            var allCategories = this.SetupCategories();
            this.AvailableCategories = new List<Category>();
            this.AvailableCategories.AddRange(allCategories);
        }

        private void FillProducts()
        {
            this.AvailableProducts = new List<Product>();
            this.AvailableChannels = new List<Channel>();
            this.AvailableProductTypes = new EditableList<ProductType>();

            var localizedName = new LocalizedString() {{"en", "apricot"}, {"de", "Aprikose"}};
            var localizedA = new LocalizedString {{"en", $"A{randomInt}"}};
            var localizedB = new LocalizedString {{"en", $"B{randomInt}"}};
            var localizedC = new LocalizedString {{"en", $"C{randomInt}"}};

            var categoryA = this.AvailableCategories.FirstOrDefault(c => c.ExternalId == "A");
            var categoryA1 = this.AvailableCategories.FirstOrDefault(c => c.ExternalId == "A1");
            var categoryB = this.AvailableCategories.FirstOrDefault(c => c.ExternalId == "B");
            var categoryB1 = this.AvailableCategories.FirstOrDefault(c => c.ExternalId == "B1");
            var categoryC = this.AvailableCategories.FirstOrDefault(c => c.ExternalId == "C");
            var productType1 = this.CreateNewProductType();
            var taxCategory1 = this.CreateNewTaxCategory();
            var state1 = this.CreateNewState();
            var channel1 = this.CreateNewChannel(ChannelRole.InventorySupply);
            var channel2 = this.CreateNewChannel(ChannelRole.InventorySupply);
            var channel3 = this.CreateNewChannel(ChannelRole.InventorySupply);
            var searchKeywordsList = new List<SearchKeywords> {new SearchKeywords {Text = "jeans"}};
            var searchKeywords1 = new Dictionary<string, List<SearchKeywords>> {{"en", searchKeywordsList}};

            var productWithLocalizedName = this.CreateProductWithLocalizedName(localizedName);
            var productWithoutCategories = this.CreateProduct();
            var productInCategoryA = this.CreateProduct(productCategory: categoryA);
            var productInCategoryA1 = this.CreateProduct(productCategory: categoryA1);
            var productInCategoryB = this.CreateProduct(productCategory: categoryB);
            var productInCategoryB1 = this.CreateProduct(productCategory: categoryB1);
            var productInCategoryC = this.CreateProduct(productCategory: categoryC);

            var randKey = TestingUtility.RandomString(10);
            var productWithInfo = this.CreateCustomizedProduct(sku: randKey, productKey: randKey,
                productVariantKey: randKey, taxCategory: taxCategory1, state: state1, searchKeywords: searchKeywords1);
            var productWithoutInfo = this.CreateCustomizedProduct(sku: null, productKey: null, productVariantKey: null,
                taxCategory: null, state: null, searchKeywords: null);
            var productWithProductType = this.CreateProduct(productType: productType1);

            var review1 = this.CreateProductReview(productWithInfo.Key, 1);
            var review2 = this.CreateProductReview(productWithInfo.Key, 3);
            productWithInfo = this.productFixture.GetProductById(productWithInfo.Id);

            var product30 = this.CreateCustomizedProduct(
                productKey: $"p30_{serviceProviderFixture.TestId}",
                sku: $"p30_{serviceProviderFixture.TestId}",
                productName: localizedB,
                productVariantAvailability: new ProductVariantAvailability
                {
                    AvailableQuantity = 0
                },
                supplyChannel: channel1,
                prices: new List<PriceDraft> {TestingUtility.Euro30, TestingUtility.EuroScoped40},
                attributes: new List<Attribute>
                {
                    new NumberAttribute {Name = "number-attribute-name", Value = 30},
                    new MoneyAttribute {Name = "money-attribute-name", Value = TestingUtility.Money30},
                    new EnumAttribute
                    {
                        Name = "enum-attribute-name",
                        Value = new PlainEnumValue {Key = "enum-key-1", Label = "enum-label-1"}
                    },
                    new ReferenceAttribute
                    {
                        Name = "reference-attribute-name",
                        Value = new Reference {Id = productType1.Id, TypeId = ReferenceTypeId.ProductType}
                    }
                }
            );

            var product50 = this.CreateCustomizedProduct(
                productKey: $"p50_{serviceProviderFixture.TestId}",
                sku: $"p50_{serviceProviderFixture.TestId}",
                productName: localizedC,
                productVariantAvailability: new ProductVariantAvailability
                {
                    AvailableQuantity = 3,
                    RestockableInDays = 3
                },
                supplyChannel: channel1,
                prices: new List<PriceDraft> {TestingUtility.Euro50, TestingUtility.EuroScoped60},
                attributes: new List<Attribute>
                {
                    new NumberAttribute {Name = "number-attribute-name", Value = 50},
                    new MoneyAttribute {Name = "money-attribute-name", Value = TestingUtility.Money50},
                    new EnumAttribute
                    {
                        Name = "enum-attribute-name",
                        Value = new PlainEnumValue {Key = "enum-key-2", Label = "enum-label-2"}
                    }
                }
            );

            var product70 = this.CreateCustomizedProduct(
                productKey: $"p70_{serviceProviderFixture.TestId}",
                sku: $"p70_{serviceProviderFixture.TestId}",
                productName: localizedA,
                productVariantAvailability: new ProductVariantAvailability
                {
                    AvailableQuantity = 6,
                    RestockableInDays = 6
                },
                supplyChannel: channel1,
                prices: new List<PriceDraft> {TestingUtility.Euro70, TestingUtility.EuroScoped80},
                attributes: new List<Attribute>
                {
                    new NumberAttribute {Name = "number-attribute-name", Value = 70},
                    new MoneyAttribute {Name = "money-attribute-name", Value = TestingUtility.Money70},
                    new EnumAttribute
                    {
                        Name = "enum-attribute-name",
                        Value = new PlainEnumValue {Key = "enum-key-3", Label = "enum-label-3"}
                    }
                }
            );

            var product90WithDiscount = this.CreateCustomizedProduct(
                prices: new List<PriceDraft> {TestingUtility.Euro90, TestingUtility.EuroScoped100});
            var productDiscount =
                this.CreateProductDiscountOfAbsoluteValue(product90WithDiscount.Id, TestingUtility.DiscountOf5Euro);


            var product30InChannel2 = this.CreateCustomizedProduct(
                productKey: $"p30Channel2_{serviceProviderFixture.TestId}",
                sku: $"p30Channel2_{serviceProviderFixture.TestId}",
                productVariantAvailability: new ProductVariantAvailability
                {
                    AvailableQuantity = 2
                },
                supplyChannel: channel2,
                prices: new List<PriceDraft> {TestingUtility.Euro30, TestingUtility.EuroScoped40}
            );

            var product30InChannel3 = this.CreateCustomizedProduct(
                productKey: $"p30Channel3_{serviceProviderFixture.TestId}",
                sku: $"p30Channel3_{serviceProviderFixture.TestId}",
                productVariantAvailability: new ProductVariantAvailability
                {
                    AvailableQuantity = 4
                },
                supplyChannel: channel3,
                prices: new List<PriceDraft> {TestingUtility.Euro30, TestingUtility.EuroScoped40}
            );

            // Product Available without channel
            var product30WithoutChannel = this.CreateCustomizedProduct(
                productKey: $"p30NoChannel_{serviceProviderFixture.TestId}",
                sku: $"p30NoChannel_{serviceProviderFixture.TestId}",
                productVariantAvailability: new ProductVariantAvailability
                {
                    AvailableQuantity = 0
                },
                supplyChannel: null,
                prices: new List<PriceDraft> {TestingUtility.Euro30, TestingUtility.EuroScoped40}
            );
            var product50WithoutChannel = this.CreateCustomizedProduct(
                productKey: $"p50NoChannel_{serviceProviderFixture.TestId}",
                sku: $"p50NoChannel_{serviceProviderFixture.TestId}",
                productVariantAvailability: new ProductVariantAvailability
                {
                    AvailableQuantity = 3
                },
                supplyChannel: null,
                prices: new List<PriceDraft> {TestingUtility.Euro30, TestingUtility.EuroScoped40}
            );
            var product70WithoutChannel = this.CreateCustomizedProduct(
                productKey: $"p70NoChannel_{serviceProviderFixture.TestId}",
                sku: $"p70NoChannel_{serviceProviderFixture.TestId}",
                productVariantAvailability: new ProductVariantAvailability
                {
                    AvailableQuantity = 6
                },
                supplyChannel: null,
                prices: new List<PriceDraft> {TestingUtility.Euro30, TestingUtility.EuroScoped40}
            );

            //Creating reviews to product30
            this.CreateProductReview(product30.Key, 1);
            this.CreateProductReview(product30.Key, 3);

            //Creating reviews to product50
            this.CreateProductReview(product50.Key, 3);
            this.CreateProductReview(product50.Key, 5);

            //Creating reviews to product70
            this.CreateProductReview(product70.Key, 1);
            this.CreateProductReview(product70.Key, 1);

            //Add created products to the list
            this.AvailableProducts.Add(productWithLocalizedName);
            this.AvailableProducts.Add(productWithoutCategories);
            this.AvailableProducts.Add(productInCategoryA);
            this.AvailableProducts.Add(productInCategoryA1);
            this.AvailableProducts.Add(productInCategoryB);
            this.AvailableProducts.Add(productInCategoryB1);
            this.AvailableProducts.Add(productInCategoryC);
            this.AvailableProducts.Add(productWithInfo);
            this.AvailableProducts.Add(productWithoutInfo);
            this.AvailableProducts.Add(productWithProductType);
            this.AvailableProducts.Add(product30);
            this.AvailableProducts.Add(product50);
            this.AvailableProducts.Add(product70);
            this.AvailableProducts.Add(product90WithDiscount);
            this.AvailableProducts.Add(product30InChannel2);
            this.AvailableProducts.Add(product30InChannel3);
            this.AvailableProducts.Add(product30WithoutChannel);
            this.AvailableProducts.Add(product50WithoutChannel);
            this.AvailableProducts.Add(product70WithoutChannel);

            //Add channels
            this.AvailableChannels.Add(channel1);
            this.AvailableChannels.Add(channel2);
            this.AvailableChannels.Add(channel3);
            //Add Product Types
            this.AvailableProductTypes.Add(productType1);
        }

        public Product FindProduct(Func<Product, bool> predicate)
        {
            return this.AvailableProducts.FirstOrDefault(predicate);
        }

        public Category FindCategory(Func<Category, bool> predicate)
        {
            return this.AvailableCategories.FirstOrDefault(predicate);
        }
    }
}
