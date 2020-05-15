using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.Domain.Channels;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.InventoryEntries;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.Domain.ProductDiscounts;
using commercetools.Sdk.Domain.ProductProjections;
using commercetools.Sdk.Domain.Products.Attributes;
using commercetools.Sdk.Domain.Reviews;
using commercetools.Sdk.Domain.States;
using commercetools.Sdk.Domain.TaxCategories;
using Xunit;
using Xunit.Abstractions;
using static commercetools.Sdk.IntegrationTests.GenericFixture;
using static commercetools.Sdk.IntegrationTests.Products.ProductsFixture;
using static commercetools.Sdk.IntegrationTests.ProductTypes.ProductTypesFixture;
using static commercetools.Sdk.IntegrationTests.Categories.CategoriesFixture;
using static commercetools.Sdk.IntegrationTests.Channels.ChannelsFixture;
using static commercetools.Sdk.IntegrationTests.States.StatesFixture;
using static commercetools.Sdk.IntegrationTests.TaxCategories.TaxCategoriesFixture;
using static commercetools.Sdk.IntegrationTests.ProductDiscounts.ProductDiscountsFixture;
using static commercetools.Sdk.IntegrationTests.Reviews.ReviewsFixture;
using static commercetools.Sdk.IntegrationTests.Inventory.InventoryFixture;
using Attribute = commercetools.Sdk.Domain.Products.Attributes.Attribute;

namespace commercetools.Sdk.IntegrationTests.ProductProjectionSearch
{
    public class ProductProjectionSearchFixture
    {
        #region Keys

        public const string KeyPrefix = "EntityForSearch_New";
        public readonly string KeyLocalizedProduct = $"{KeyPrefix}_localizedProduct";
        public readonly string KeyProductWithInfo = $"{KeyPrefix}_ProductWithInfo";
        public readonly string KeyProductWithoutInfo = $"{KeyPrefix}_ProductWithoutInfo";
        public readonly string KeyProductWithPrice30 = $"{KeyPrefix}_ProductWithPrice_30";
        public readonly string KeyProductWithPrice30InChannel2 = $"{KeyPrefix}_ProductWithPrice_30_InChannel2";
        public readonly string KeyProductWithPrice30InChannel3 = $"{KeyPrefix}_ProductWithPrice_30_InChannel3";
        public readonly string KeyProductWithPrice30WithoutChannel = $"{KeyPrefix}_ProductWithPrice_30_WithoutChannel";
        public readonly string KeyProductWithPrice50WithoutChannel = $"{KeyPrefix}_ProductWithPrice_50_WithoutChannel";
        public readonly string KeyProductWithPrice70WithoutChannel = $"{KeyPrefix}_ProductWithPrice_70_WithoutChannel";
        public readonly string KeyProductWithPrice50 = $"{KeyPrefix}_ProductWithPrice_50";
        public readonly string KeyProductWithPrice70 = $"{KeyPrefix}_ProductWithPrice_70";
        public readonly string KeyProductWithPrice90WithDiscount = $"{KeyPrefix}_ProductWithPrice_90_WithDiscount";
        public readonly string KeyProductType = $"{KeyPrefix}_ProductType";
        public readonly string KeyState = $"{KeyPrefix}_State";
        public readonly string KeyTaxCategory = $"{KeyPrefix}_TaxCategory";
        public readonly string KeyProductDiscount = $"{KeyPrefix}_ProductDiscount";
        public readonly string DescriptionCategory = $"{KeyPrefix}_Category";
        public readonly string DescriptionChannel = $"{KeyPrefix}_Channel";
        public readonly string DescriptionProductDiscount = $"{KeyPrefix}_ProductDiscount";
        public readonly string AuthorReview = $"{KeyPrefix}_Review";

        #endregion

        #region Variables

        public readonly ServiceProviderFixture ServiceProviderFixture;
        private readonly IClient client;

        private const int ExpectedProductsCount = 17;
        private const int ExpectedCategoriesCount = 9;
        private const int ExpectedChannelsCount = 3;
        private const int ExpectedReviewsCount = 6;
        private const int ExpectedProductDiscounts = 1;
        private const int ExpectedInventoryEntriesCount = 8;
        private const bool SearchForStaged = true;

        public bool AvailableForSearch { get; set; }
        public ProductType AvailableProductType { get; set; }

        private State AvailableState { get; set; }

        private TaxCategory AvailableTaxCategory { get; set; }
        private List<ProductProjection> AvailableProducts { get; set; }

        public List<Category> AvailableCategories { get; set; }

        public List<Channel> AvailableChannels { get; set; }

        #endregion

        #region Constructors

        public ProductProjectionSearchFixture(IMessageSink diagnosticMessageSink)
        {
            //create serviceProviderFixture Instance
            this.ServiceProviderFixture = new ServiceProviderFixture(diagnosticMessageSink);
            this.client = this.ServiceProviderFixture.GetService<IClient>();
            Setup();
        }

        #endregion

        #region Functions

        //Check if all search entities (ByCount) are created and indexed or not
        //if they indexed then fill local lists with them
        //else create them and wait till they indexed and then fill local lists with them
        private void Setup()
        {
            CreateEntitiesIfNotExists();

            FillEntitiesLists();

            AvailableForSearch = LoopTillProductsIndexed();
        }

        /// <summary>
        /// Create Entities if not exists
        /// </summary>
        private void CreateEntitiesIfNotExists()
        {
            AvailableProductType = CreateOrRetrieveProductType();
            AvailableState = CreateOrRetrieveState();
            AvailableTaxCategory = CreateOrRetrieveTaxCategory();

            //Categories
            CreateCategoriesIfNotExists();

            //Channels
            CreateChannelsIfNotExists();

            FillEntitiesLists(); //before creating products, fill local lists like categories,channels, ..etc

            //Products
            CreateProductsIfNotExists();
        }

        private void CreateCategoriesIfNotExists()
        {
            var categoriesExists = CheckIfEntitiesExists<Category>(
                c => c.MetaDescription["en"] == DescriptionCategory.valueOf(), ExpectedCategoriesCount);

            if (categoriesExists)
                return;

            var root = new List<string> {"A", "B", "C"};
            var numberOfChild = 2;

            foreach (var rootCat in root)
            {
                var rootCategory = CreateOrRetrieveCategory(rootCat);
                //create child categories
                for (var i = 1; i <= numberOfChild; i++)
                {
                    CreateOrRetrieveCategory($"{rootCat}{i}", rootCategory);
                }
            }
        }

        private void CreateChannelsIfNotExists()
        {
            var channelsExists = CheckIfEntitiesExists<Channel>(
                c => c.Description["en"] == DescriptionChannel.valueOf(), ExpectedChannelsCount);

            if (channelsExists)
                return;

            var channels = new List<string> {"A", "B", "C"};

            foreach (var channelLabel in channels)
            {
                CreateOrRetrieveChannel(channelLabel);
            }
        }

        private void CreateProductsIfNotExists()
        {
            var productsExists = CheckIfEntitiesExists<Product>(
                p => p.ProductType.Id == AvailableProductType.Id.valueOf(),
                ExpectedProductsCount);

            var reviewsExists = CheckIfEntitiesExists<Review>(
                r => r.AuthorName == AuthorReview.valueOf(),
                ExpectedReviewsCount);

            var productDiscountsExists = CheckIfEntitiesExists<ProductDiscount>(
                d => d.Description["en"] == DescriptionProductDiscount.valueOf(),
                ExpectedProductDiscounts);

            if (productsExists && reviewsExists && productDiscountsExists)
                return;

            //product with localizedName
            CreateOrRetrieveLocalizedProduct();
            CreateProductsInCategories();
            CreateProductsWithPrices();
            CreateOtherProducts();
        }

        private void CreateProductsInCategories()
        {
            var categoryA = this.AvailableCategories.FirstOrDefault(c => c.ExternalId == "A");
            var categoryA1 = this.AvailableCategories.FirstOrDefault(c => c.ExternalId == "A1");
            var categoryB = this.AvailableCategories.FirstOrDefault(c => c.ExternalId == "B");
            var categoryB1 = this.AvailableCategories.FirstOrDefault(c => c.ExternalId == "B1");
            var categoryC = this.AvailableCategories.FirstOrDefault(c => c.ExternalId == "C");

            CreateOrRetrieveProductInCategory(categoryA);
            CreateOrRetrieveProductInCategory(categoryA1);
            CreateOrRetrieveProductInCategory(categoryB);
            CreateOrRetrieveProductInCategory(categoryB1);
            CreateOrRetrieveProductInCategory(categoryC);
        }

        private void CreateProductsWithPrices()
        {
            var channel1 = AvailableChannels[0];
            var channel2 = AvailableChannels[1];
            var channel3 = AvailableChannels[2];


            //Product30
            var product30Prices = new List<PriceDraft>
                {TestingUtility.Euro30, TestingUtility.EuroScoped40};
            var product30EnumValue = new PlainEnumValue {Key = "enum-key-1", Label = "enum-label-1"};
            var product30Availability = new ProductVariantAvailability
            {
                AvailableQuantity = 0
            };
            var product30 = CreateOrRetrievePricedProduct(KeyProductWithPrice30, product30Prices,
                TestingUtility.Money30, 30, product30EnumValue);
            var product30InChannel2 = CreateOrRetrievePricedProduct(KeyProductWithPrice30InChannel2, product30Prices);
            var product30InChannel3 = CreateOrRetrievePricedProduct(KeyProductWithPrice30InChannel3, product30Prices);


            //product50
            var product50Prices = new List<PriceDraft>
                {TestingUtility.Euro50, TestingUtility.EuroScoped60};
            var product50EnumValue = new PlainEnumValue {Key = "enum-key-2", Label = "enum-label-2"};
            var product50Availability = new ProductVariantAvailability
            {
                AvailableQuantity = 3,
                RestockableInDays = 3
            };
            var product50 = CreateOrRetrievePricedProduct(KeyProductWithPrice50, product50Prices,
                TestingUtility.Money50, 50, product50EnumValue);


            //product70
            var product70Prices = new List<PriceDraft>
                {TestingUtility.Euro70, TestingUtility.EuroScoped80};
            var product70EnumValue = new PlainEnumValue {Key = "enum-key-3", Label = "enum-label-3"};
            var product70Availability = new ProductVariantAvailability
            {
                AvailableQuantity = 6,
                RestockableInDays = 6
            };
            var product70 = CreateOrRetrievePricedProduct(KeyProductWithPrice70, product70Prices,
                TestingUtility.Money70, 70, product70EnumValue);

            //product90WithDiscount
            var product90WithDiscountPrices = new List<PriceDraft>
                {TestingUtility.Euro90, TestingUtility.EuroScoped100};

            var product90WithDiscount =
                CreateOrRetrievePricedProduct(KeyProductWithPrice90WithDiscount, product90WithDiscountPrices);
            var productDiscount =
                CreateOrRetrieveProductDiscountOfAbsoluteValue(product90WithDiscount.Id,
                    TestingUtility.DiscountOf5Euro);


            // Product Available without channel
            var product30WithoutChannel = CreateOrRetrievePricedProduct(KeyProductWithPrice30WithoutChannel, product30Prices);
            var product50WithoutChannel = CreateOrRetrievePricedProduct(KeyProductWithPrice50WithoutChannel, product50Prices);
            var product70WithoutChannel = CreateOrRetrievePricedProduct(KeyProductWithPrice70WithoutChannel, product70Prices);

            //Creating reviews to product30
            CreateOrRetrieveReview(product30, 1, "1");
            CreateOrRetrieveReview(product30, 3, "2");

            //Creating reviews to product50
            CreateOrRetrieveReview(product50, 3, "1");
            CreateOrRetrieveReview(product50, 5, "2");

            //Creating reviews to product70
            CreateOrRetrieveReview(product70, 1, "1");
            CreateOrRetrieveReview(product70, 1, "2");


            //Inventory
            CreateOrRetrieveInventoryEntry(product30, product30Availability, channel1);
            CreateOrRetrieveInventoryEntry(product50, product50Availability, channel1);
            CreateOrRetrieveInventoryEntry(product70, product70Availability, channel1);

            CreateOrRetrieveInventoryEntry(product30InChannel2,
                new ProductVariantAvailability { AvailableQuantity = 2}, channel2);
            CreateOrRetrieveInventoryEntry(product30InChannel3,
                new ProductVariantAvailability { AvailableQuantity = 4}, channel3);
            CreateOrRetrieveInventoryEntry(product30WithoutChannel,
                new ProductVariantAvailability { AvailableQuantity = 0}, null);
            CreateOrRetrieveInventoryEntry(product50WithoutChannel,
                new ProductVariantAvailability { AvailableQuantity = 3}, null);
            CreateOrRetrieveInventoryEntry(product70WithoutChannel,
                new ProductVariantAvailability { AvailableQuantity = 6}, null);
        }

        private void CreateOtherProducts()
        {
            var searchKeywordsList = new List<SearchKeywords> {new SearchKeywords {Text = "jeans"}};
            var searchKeywords1 = new Dictionary<string, List<SearchKeywords>> {{"en", searchKeywordsList}};

            var productWithInfo = CreateOrRetrieveByKey<Product, ProductDraft>(client, KeyProductWithInfo,
                new ProductDraft(), draft =>
                {
                    var productDraft = DefaultProductDraftWithProductType(draft, AvailableProductType);
                    productDraft.Key = KeyProductWithInfo;
                    productDraft.State = AvailableState.ToKeyResourceIdentifier();
                    productDraft.TaxCategory = AvailableTaxCategory.ToKeyResourceIdentifier();
                    productDraft.SearchKeywords = searchKeywords1;
                    return productDraft;
                });

            var productWithoutInfo = CreateOrRetrieveByKey<Product, ProductDraft>(client, KeyProductWithoutInfo,
                new ProductDraft(), draft =>
                {
                    var productDraft = DefaultProductDraftWithProductType(draft, AvailableProductType);
                    productDraft.Key = null;
                    productDraft.MasterVariant.Prices = new List<PriceDraft>(); //empty prices
                    productDraft.MasterVariant.Sku = null;
                    productDraft.MasterVariant.Key = null;
                    productDraft.MasterVariant.Attributes = null;
                    productDraft.State = null;
                    productDraft.TaxCategory = null;
                    return productDraft;
                });
        }


        /// <summary>
        /// Fill local lists with entities, like AvailableCategories, AvailableChannels
        /// </summary>
        private void FillEntitiesLists()
        {
            FillCategoriesList();
            FillChannelsList();
        }

        /// <summary>
        /// Fill AvailableCategories if it's empty with all categories with the same meta description
        /// </summary>
        private void FillCategoriesList()
        {
            if (AvailableCategories == null || AvailableCategories.Count == 0)
            {
                AvailableCategories =
                    FillEntitiesList<Category>(c => c.MetaDescription["en"] == DescriptionCategory.valueOf());
            }
        }

        /// <summary>
        /// Fill AvailableChannels if it's empty with all channels with the same description
        /// </summary>
        private void FillChannelsList()
        {
            if (AvailableChannels == null || AvailableChannels.Count == 0)
            {
                AvailableChannels = FillEntitiesList<Channel>(c => c.Description["en"] == DescriptionChannel.valueOf());
            }
        }

        private List<T> FillEntitiesList<T>(Expression<Func<T, bool>> expression)
        {
            var queryCommand = new QueryCommand<T>();
            queryCommand.Where(expression);
            var returnedSet = client.ExecuteAsync(queryCommand).Result;
            var list = returnedSet.Results;
            return list;
        }


        public ProductProjection FindProduct(Func<ProductProjection, bool> predicate)
        {
            if (AvailableProducts == null || AvailableProducts.Count == 0)
                return null;
            return AvailableProducts.FirstOrDefault(predicate);
        }

        public Category FindCategory(Func<Category, bool> predicate)
        {
            if (AvailableCategories == null || AvailableCategories.Count == 0)
                return null;
            return this.AvailableCategories.FirstOrDefault(predicate);
        }

        #endregion

        #region HelperMethods

        private bool CheckIfEntitiesExists<T>(Expression<Func<T, bool>> expression, int expectedCount)
        {
            var queryCommand = new QueryCommand<T>();
            queryCommand.Where(expression);
            var returnedSet = client.ExecuteAsync(queryCommand).Result;
            return returnedSet.Count == expectedCount;
        }

        /// <summary>
        /// check if all the products used in search tests are indexed
        /// </summary>
        /// <returns>true if the products used in search tests are indexed</returns>
        private bool CheckIfProductsIndexed()
        {
            if (AvailableProductType == null)
                return false;

            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(SearchForStaged);
            searchRequest.FilterQuery(p => p.ProductType.Id == AvailableProductType.Id.valueOf());
            var searchResults = client.ExecuteAsync(searchRequest).Result;
            var indexed = searchResults.Count >= ExpectedProductsCount;
            if (indexed)
            {
                AvailableProducts = searchResults.Results;
            }

            return indexed;
        }

        private bool LoopTillProductsIndexed()
        {
            try
            {
                AssertEventually(() =>
                {
                    var indexed = CheckIfProductsIndexed();
                    Assert.True(indexed);
                });
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        private ProductType CreateOrRetrieveProductType()
        {
            var productType = CreateOrRetrieveByKey<ProductType, ProductTypeDraft>(client, KeyProductType,
                new ProductTypeDraft(),
                productTypeDraft => DefaultProductTypeDraftWithKey(productTypeDraft, KeyProductType));
            return productType;
        }

        private Category CreateOrRetrieveCategory(string categoryLabel, Category rootCategory = null)
        {
            var categoryKey = $"{KeyPrefix}_Category_{categoryLabel}";
            var category = CreateOrRetrieveByKey<Category, CategoryDraft>(client, categoryKey,
                new CategoryDraft(), draft =>
                {
                    var categoryDraft = DefaultCategoryDraftWithKey(draft, categoryKey);
                    categoryDraft.Name = new LocalizedString {{"en", categoryKey}};
                    categoryDraft.Slug = new LocalizedString {{"en", categoryKey}};
                    categoryDraft.ExternalId = categoryLabel;
                    categoryDraft.Parent = rootCategory?.ToKeyResourceIdentifier();
                    categoryDraft.MetaDescription = new LocalizedString {{"en", DescriptionCategory}};
                    return categoryDraft;
                });
            return category;
        }

        private Channel CreateOrRetrieveChannel(string channelLabel)
        {
            var channelKey = $"{KeyPrefix}_Channel_{channelLabel}";
            var channel = CreateOrRetrieveByKey<Channel, ChannelDraft>(client, channelKey,
                new ChannelDraft(), draft =>
                {
                    var channelDraft = DefaultChannelDraftWithKey(draft, channelKey);
                    channelDraft.Name = new LocalizedString {{"en", channelKey}};
                    channelDraft.Roles = new List<ChannelRole>
                    {
                        ChannelRole.InventorySupply
                    };
                    channelDraft.Description = new LocalizedString {{"en", DescriptionChannel}};
                    return channelDraft;
                });
            return channel;
        }

        private State CreateOrRetrieveState()
        {
            var state = CreateOrRetrieveByKey<State, StateDraft>(client, KeyState, new StateDraft(),
                draft =>
                {
                    var stateDraft = DefaultStateDraftWithKey(draft, KeyState);
                    stateDraft.Name = new LocalizedString() {{"en", KeyState}};
                    return stateDraft;
                });
            return state;
        }

        private Review CreateOrRetrieveReview(Product product, int rating, string reviewCode)
        {
            var reviewForProductKey = $"{KeyPrefix}_Review_{reviewCode}ForProduct_{product.Key}";
            var review = CreateOrRetrieveByKey<Review, ReviewDraft>(client, reviewForProductKey, new ReviewDraft(),
                draft =>
                {
                    var reviewDraft = DefaultReviewDraftWithKey(draft, reviewForProductKey);
                    reviewDraft.Target = product.ToKeyResourceIdentifier();
                    reviewDraft.Rating = rating;
                    reviewDraft.Title = $"Review For Product with key: {product.Key}";
                    reviewDraft.AuthorName = AuthorReview;
                    return reviewDraft;
                });
            return review;
        }

        private TaxCategory CreateOrRetrieveTaxCategory()
        {
            var taxCategory = CreateOrRetrieveByKey<TaxCategory, TaxCategoryDraft>(client, KeyTaxCategory,
                new TaxCategoryDraft(),
                draft =>
                {
                    var taxCategoryDraft = DefaultTaxCategoryDraftWithKey(draft, KeyTaxCategory);
                    taxCategoryDraft.Name = KeyTaxCategory;
                    return taxCategoryDraft;
                });
            return taxCategory;
        }

        private InventoryEntry CreateOrRetrieveInventoryEntry(Product product,
            ProductVariantAvailability variantAvailability, Channel supplyChannel = null)
        {
            var inventoryEntrySku = $"{KeyPrefix}_InventoryEntryOfProduct_{product.Key}";
            var inventoryEntry = CreateOrRetrieveBySku(client, inventoryEntrySku, new InventoryEntryDraft(),
                draft =>
                {
                    var inventoryEntryDraft = DefaultInventoryEntryDraftWithSku(draft, inventoryEntrySku);
                    inventoryEntryDraft.Sku = product.MasterData.Staged.MasterVariant.Sku;
                    inventoryEntryDraft.SupplyChannel = supplyChannel?.ToReference();
                    inventoryEntryDraft.QuantityOnStock = variantAvailability.AvailableQuantity;
                    inventoryEntryDraft.RestockableInDays = variantAvailability.RestockableInDays;
                    return inventoryEntryDraft;
                });
            return inventoryEntry;
        }

        private ProductDiscount CreateOrRetrieveProductDiscountOfAbsoluteValue(string productId, int discountCentAmount)
        {
            var money = new Money() {CurrencyCode = "EUR", CentAmount = discountCentAmount};

            var productDiscountValue = new AbsoluteProductDiscountValue()
            {
                Money = new List<Money>() {money}
            };

            var productDiscount = CreateOrRetrieveByKey<ProductDiscount, ProductDiscountDraft>(client,
                KeyProductDiscount,
                new ProductDiscountDraft(),
                draft =>
                {
                    var productDiscountDraft = DefaultProductDiscountDraftWithKey(draft, KeyProductDiscount);

                    productDiscountDraft.Value = productDiscountValue;
                    productDiscountDraft.ValidFrom = DateTime.Today;
                    productDiscountDraft.ValidUntil = DateTime.Today.AddYears(2);
                    productDiscountDraft.IsActive = true;
                    productDiscountDraft.Description = new LocalizedString {{"en", DescriptionProductDiscount}};
                    productDiscountDraft.SetPredicate(product => product.ProductId() == productId);

                    return productDiscountDraft;
                });
            return productDiscount;
        }

        private Product CreateOrRetrieveLocalizedProduct()
        {
            var localizedName = new LocalizedString() {{"en", "apricot"}, {"de", "Aprikose"}};
            var product = CreateOrRetrieveByKey<Product, ProductDraft>(client, KeyLocalizedProduct,
                new ProductDraft(), draft =>
                {
                    var productDraft = DefaultProductDraftWithProductType(draft, AvailableProductType);
                    productDraft.Name = localizedName;
                    productDraft.Key = KeyLocalizedProduct;
                    return productDraft;
                });
            return product;
        }

        private Product CreateOrRetrieveProductInCategory(Category category)
        {
            var productInCategoryKey = $"{KeyPrefix}_ProductInCategory_{category.ExternalId}";

            var product = CreateOrRetrieveByKey<Product, ProductDraft>(client, productInCategoryKey,
                new ProductDraft(), draft =>
                {
                    var productDraft = DefaultProductDraftWithProductType(draft, AvailableProductType);
                    productDraft.Categories = new List<IReference<Category>>()
                    {
                        category.ToKeyResourceIdentifier()
                    };
                    productDraft.Key = productInCategoryKey;
                    return productDraft;
                });
            return product;
        }

        private Product CreateOrRetrievePricedProduct(string key, List<PriceDraft> prices,
            Money moneyAttrValue = null, double? numberAttrValue = null, PlainEnumValue enumAttrValue = null)
        {
            var product = CreateOrRetrieveByKey<Product, ProductDraft>(client, key,
                new ProductDraft(), draft =>
                {
                    var productDraft = DefaultProductDraftWithProductType(draft, AvailableProductType);
                    productDraft.Key = key;
                    productDraft.MasterVariant.Sku = key;
                    productDraft.Name = new LocalizedString {{"en", key}};
                    productDraft.MasterVariant.Prices = prices;
                    if (enumAttrValue != null && numberAttrValue.HasValue)
                    {
                        productDraft.MasterVariant.Attributes = new List<Attribute>
                        {
                            new NumberAttribute {Name = "number-attribute-name", Value = numberAttrValue.Value},
                            new MoneyAttribute {Name = "money-attribute-name", Value = moneyAttrValue},
                            new EnumAttribute {Name = "enum-attribute-name", Value = enumAttrValue},
                            new ReferenceAttribute
                            {
                                Name = "reference-attribute-name",
                                Value = new Reference
                                {
                                    Id = AvailableProductType.Id,
                                    TypeId = ReferenceTypeId.ProductType
                                }
                            }
                        };
                    }

                    return productDraft;
                });
            return product;
        }

        #endregion
    }
}
