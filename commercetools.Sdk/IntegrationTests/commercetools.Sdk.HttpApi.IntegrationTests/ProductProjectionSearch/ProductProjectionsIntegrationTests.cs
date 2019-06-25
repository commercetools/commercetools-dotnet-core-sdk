using System;
using System.Collections.Generic;
using System.Linq;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Channels;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.Domain.ProductProjections;
using commercetools.Sdk.Domain.Products.Attributes;
using commercetools.Sdk.Domain.Query;
using Xunit;
using Attribute = commercetools.Sdk.Domain.Products.Attributes.Attribute;

namespace commercetools.Sdk.HttpApi.IntegrationTests.ProductProjectionSearch
{
    [Collection("Integration Tests")]
    public class ProductProjectionSearchIntegrationTests : IClassFixture<ServiceProviderFixture>, IDisposable
    {
        private readonly ProductProjectionSearchFixture productProjectionsFixture;

        public ProductProjectionSearchIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.productProjectionsFixture = new ProductProjectionSearchFixture(serviceProviderFixture);
        }

        public void Dispose()
        {
            this.productProjectionsFixture.Dispose();
        }

        [Fact]
        public void SearchByFullLocale()
        {
            //update current project to support these languages
            var project = this.productProjectionsFixture.ChangeProjectLanguages(new List<string> {"en", "de"});
            Assert.Equal(2, project.Languages.Count);
            Assert.True(project.Languages.Contains("en") && project.Languages.Contains("de"));

            var rand = TestingUtility.RandomInt();

            var localizedName = new LocalizedString()
            {
                {"en", $"apricot_{rand}"},
                {"de", $"Aprikose_{rand}"}
            };
            var product = this.productProjectionsFixture.CreateProductWithLocalizedName(localizedName);
            var searchParams = new ProductProjectionSearchParameters
            {
                Text = new TextSearch
                {
                    Term = localizedName["de"],
                    Language = "de"
                }
            };
            var searchRequestByGermanLanguage = new SearchProductProjectionsCommand(searchParams);
            searchRequestByGermanLanguage.SetStaged(true);


            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();
                var searchResults = commerceToolsClient.ExecuteAsync(searchRequestByGermanLanguage).Result;
                Assert.Single(searchResults.Results);
                Assert.Equal(product.Id, searchResults.Results[0].Id);
                Assert.Equal(localizedName["en"], searchResults.Results[0].Name["en"]);
            });
        }

        [Fact]
        public void SearchWithFuzzyLevel()
        {
            var localizedName = new LocalizedString()
            {
                {"en", "abcdefgh"}
            };
            var product = this.productProjectionsFixture.CreateProductWithLocalizedName(localizedName);
            var searchParams = new ProductProjectionSearchParameters
            {
                Text = new TextSearch
                {
                    Term = "abcdfgh",
                    Language = "en"
                },
                Fuzzy = true,
                FuzzyLevel = 1
            };
            var searchRequestByGermanLanguage = new SearchProductProjectionsCommand(searchParams);
            searchRequestByGermanLanguage.SetStaged(true);


            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();
                var searchResults = commerceToolsClient.ExecuteAsync(searchRequestByGermanLanguage).Result;
                Assert.Single(searchResults.Results);
                Assert.Equal(product.Id, searchResults.Results[0].Id);
                Assert.Equal(localizedName["en"], searchResults.Results[0].Name["en"]);
            });
        }

        #region Filters

        [Fact]
        public void FilterBySpecificCategoriesWithReferenceExpansion()
        {
            //Create 2 different categories
            var category1 = this.productProjectionsFixture.CreateNewCategory();
            var category2 = this.productProjectionsFixture.CreateNewCategory();

            var category1Products = new List<Product>();
            //Create 2 products with first category
            for (var i = 0; i < 2; i++)
            {
                var product = this.productProjectionsFixture.CreateProduct(productCategory: category1);
                category1Products.Add(product);
            }

            //Create 1 product with second category
            var category2Product = this.productProjectionsFixture.CreateProduct(productCategory: category2);

            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Categories.Any(c => c.Id == category1.Id.valueOf()));
            searchRequest.Expand(p => p.Categories.ExpandAll());

            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();
                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Equal(2, searchResults.Results.Count);
                Assert.Contains(searchResults.Results, p => p.Id == category1Products[0].Id);
                Assert.Contains(searchResults.Results, p => p.Id == category1Products[1].Id);
                Assert.DoesNotContain(searchResults.Results, p => p.Id == category2Product.Id);

                //check reference expansion
                Assert.Single(searchResults.Results[0].Categories);
                Assert.NotNull(searchResults.Results[0].Categories[0].Obj);
                Assert.Equal(category1.Key, searchResults.Results[0].Categories[0].Obj.Key);
            });
        }

        [Fact]
        public void FilterByCategoriesExists()
        {
            var category = this.productProjectionsFixture.CreateNewCategory();

            var productWithoutCategories = this.productProjectionsFixture.CreateProduct();
            var productWithCategories = this.productProjectionsFixture.CreateProduct(productCategory: category);

            string[] productIds = new string[]
            {
                productWithoutCategories.Id,
                productWithCategories.Id
            };


            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.FilterQuery(p => p.Categories.Exists());

            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();
                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Equal(productWithCategories.Id, searchResults.Results[0].Id);
            });
        }

        [Fact]
        public void FilterByCategoriesMissing()
        {
            var category = this.productProjectionsFixture.CreateNewCategory();

            var productWithoutCategories = this.productProjectionsFixture.CreateProduct();
            var productWithCategories = this.productProjectionsFixture.CreateProduct(productCategory: category);

            string[] productIds = new string[]
            {
                productWithoutCategories.Id,
                productWithCategories.Id
            };


            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.FilterQuery(p => p.Categories.Missing());

            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();
                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Equal(productWithoutCategories.Id, searchResults.Results[0].Id);
            });
        }

        [Fact]
        public void FilterBySpecificCategorySubtree()
        {
            //Arrange
            var allCategories = this.productProjectionsFixture.SetupCategories();
            Assert.True(allCategories.Count > 0);

            var categoryA = allCategories.FirstOrDefault(c => c.ExternalId == "A");
            var categoryA1 = allCategories.FirstOrDefault(c => c.ExternalId == "A1");
            var categoryB = allCategories.FirstOrDefault(c => c.ExternalId == "B");

            Assert.NotNull(categoryA);
            Assert.NotNull(categoryA1);
            Assert.NotNull(categoryB);

            var productA = this.productProjectionsFixture.CreateProduct(productCategory: categoryA);
            var productA1 = this.productProjectionsFixture.CreateProduct(productCategory: categoryA1);
            var productB = this.productProjectionsFixture.CreateProduct(productCategory: categoryB);

            //Act (filter products that belong to categoryA or any of its descendant categories)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Categories.Any(c => c.Id.Subtree(categoryA.Id.valueOf())));


            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();
                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Equal(2, searchResults.Results.Count);
                Assert.Contains(searchResults.Results, p => p.Id == productA.Id);
                Assert.Contains(searchResults.Results, p => p.Id == productA1.Id);
                Assert.DoesNotContain(searchResults.Results, p => p.Id == productB.Id);
            });
        }

        [Fact]
        public void FilterByMoreThanOneCategorySubtree()
        {
            //Arrange
            var allCategories = this.productProjectionsFixture.SetupCategories();
            Assert.True(allCategories.Count > 0);

            var categoryA = allCategories.FirstOrDefault(c => c.ExternalId == "A");
            var categoryA1 = allCategories.FirstOrDefault(c => c.ExternalId == "A1");
            var categoryB = allCategories.FirstOrDefault(c => c.ExternalId == "B");
            var categoryC = allCategories.FirstOrDefault(c => c.ExternalId == "C");

            Assert.NotNull(categoryA);
            Assert.NotNull(categoryA1);
            Assert.NotNull(categoryB);
            Assert.NotNull(categoryC);

            var productA = this.productProjectionsFixture.CreateProduct(productCategory: categoryA);
            var productA1 = this.productProjectionsFixture.CreateProduct(productCategory: categoryA1);
            var productB = this.productProjectionsFixture.CreateProduct(productCategory: categoryB);
            var productC = this.productProjectionsFixture.CreateProduct(productCategory: categoryC);

            //Act (filter products that belong to categoryA or categoryC or any their descendant categories)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p =>
                p.Categories.Any(c => c.Id.Subtree(categoryA.Id.valueOf()) || c.Id.Subtree(categoryC.Id.valueOf())));


            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();
                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Equal(3, searchResults.Results.Count);
                Assert.Contains(searchResults.Results, p => p.Id == productA.Id);
                Assert.Contains(searchResults.Results, p => p.Id == productA1.Id);
                Assert.Contains(searchResults.Results, p => p.Id == productC.Id);
                Assert.DoesNotContain(searchResults.Results, p => p.Id == productB.Id);
            });
        }

        [Fact]
        public void FilterByOneCategorySubtreeAndTwoIds()
        {
            //Arrange
            var allCategories = this.productProjectionsFixture.SetupCategories();
            Assert.True(allCategories.Count > 0);

            var categoryA = allCategories.FirstOrDefault(c => c.ExternalId == "A");
            var categoryA1 = allCategories.FirstOrDefault(c => c.ExternalId == "A1");
            var categoryB = allCategories.FirstOrDefault(c => c.ExternalId == "B");
            var categoryB1 = allCategories.FirstOrDefault(c => c.ExternalId == "B1");

            Assert.NotNull(categoryA);
            Assert.NotNull(categoryA1);
            Assert.NotNull(categoryB);
            Assert.NotNull(categoryB1);

            var productA = this.productProjectionsFixture.CreateProduct(productCategory: categoryA);
            var productA1 = this.productProjectionsFixture.CreateProduct(productCategory: categoryA1);
            var productB = this.productProjectionsFixture.CreateProduct(productCategory: categoryB);
            var productB1 = this.productProjectionsFixture.CreateProduct(productCategory: categoryB1);

            //Act (filter products that belong to categoryA or any of it's descendant categories and category B1)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p =>
                p.Categories.Any(c => c.Id.Subtree(categoryA.Id.valueOf()) || c.Id == categoryB1.Id.valueOf()));


            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();
                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Equal(3, searchResults.Results.Count);
                Assert.Contains(searchResults.Results, p => p.Id == productA.Id);
                Assert.Contains(searchResults.Results, p => p.Id == productA1.Id);
                Assert.Contains(searchResults.Results, p => p.Id == productB1.Id);
                Assert.DoesNotContain(searchResults.Results, p => p.Id == productB.Id);
            });
        }


        [Fact]
        public void FilterByPrice()
        {
            //Arrange
            var euro30 = TestingUtility.GetPriceDraft(30);
            var euro50 = TestingUtility.GetPriceDraft(50);
            var euro70 = TestingUtility.GetPriceDraft(70);

            var product30 = this.productProjectionsFixture.CreateCustomizedProduct(
                prices: new List<PriceDraft> {euro30});
            var product50 = this.productProjectionsFixture.CreateCustomizedProduct(
                prices: new List<PriceDraft> {euro50});
            var product70 = this.productProjectionsFixture.CreateCustomizedProduct(
                prices: new List<PriceDraft> {euro70});


            var productIds = new[] {product30.Id, product50.Id, product70.Id};


            //Act
            //(filter by specific price)
            var filterBySpecificPrice = new SearchProductProjectionsCommand();
            filterBySpecificPrice.SetStaged(true);
            filterBySpecificPrice.FilterQuery(p => p.Id.In(productIds));
            filterBySpecificPrice.FilterQuery(p =>
                p.Variants.Any(v => v.Price.Value.CentAmount == euro30.Value.CentAmount.valueOf()));

            //(filter by range price to)
            var filterByRangeToPrice = new SearchProductProjectionsCommand();
            filterByRangeToPrice.SetStaged(true);
            filterByRangeToPrice.FilterQuery(p => p.Id.In(productIds));
            filterByRangeToPrice.FilterQuery(p =>
                p.Variants.Any(v => v.Price.Value.CentAmount.Range(null, euro50.Value.CentAmount.valueOf())));

            //(filter by range price from - to)
            var filterByRangeFromToPrice = new SearchProductProjectionsCommand();
            filterByRangeFromToPrice.SetStaged(true);
            filterByRangeFromToPrice.FilterQuery(p => p.Id.In(productIds));
            filterByRangeFromToPrice.FilterQuery(p => p.Variants.Any(v => v.Price.Value.CentAmount.Range(40, 80)));

            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();

                var filterBySpecificPriceResult = commerceToolsClient.ExecuteAsync(filterBySpecificPrice).Result;
                Assert.Single(filterBySpecificPriceResult.Results);
                Assert.Contains(filterBySpecificPriceResult.Results, p => p.Id == product30.Id);

                var filterByRangeToPriceResult = commerceToolsClient.ExecuteAsync(filterByRangeToPrice).Result;
                Assert.Equal(2, filterByRangeToPriceResult.Results.Count);
                Assert.Contains(filterByRangeToPriceResult.Results, p => p.Id == product30.Id);
                Assert.Contains(filterByRangeToPriceResult.Results, p => p.Id == product50.Id);

                var filterByRangeFromToPriceResult = commerceToolsClient.ExecuteAsync(filterByRangeFromToPrice).Result;
                Assert.Equal(2, filterByRangeFromToPriceResult.Results.Count);
                Assert.Contains(filterByRangeFromToPriceResult.Results, p => p.Id == product50.Id);
                Assert.Contains(filterByRangeFromToPriceResult.Results, p => p.Id == product70.Id);
            });
        }

        [Fact]
        public void FilterByPricesMissing()
        {
            //Arrange
            var euro30 = TestingUtility.GetPriceDraft(30);
            var productWithPrices =
                this.productProjectionsFixture.CreateCustomizedProduct(prices: new List<PriceDraft> {euro30});
            var productWithoutPrices = this.productProjectionsFixture.CreateCustomizedProduct(null);

            string[] productIds = new string[]
            {
                productWithPrices.Id,
                productWithoutPrices.Id
            };

            //Act (filter only products with no price set)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.FilterQuery(p => p.Variants.Any(v => v.Prices.Missing()));

            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();
                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == productWithoutPrices.Id);
            });
        }

        [Fact]
        public void FilterByPricesExists()
        {
            //Arrange
            var euro30 = TestingUtility.GetPriceDraft(30);
            var productWithPrices =
                this.productProjectionsFixture.CreateCustomizedProduct(prices: new List<PriceDraft> {euro30});
            var productWithoutPrices = this.productProjectionsFixture.CreateCustomizedProduct(null);

            string[] productIds = new string[]
            {
                productWithPrices.Id,
                productWithoutPrices.Id
            };

            //Act (filter only products with no price set)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.FilterQuery(p => p.Variants.Any(v => v.Prices.Exists()));

            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();
                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == productWithPrices.Id);
            });
        }


        [Fact]
        public void FilterByScopedPriceValue()
        {
            //Arrange
            var prices1 = new List<PriceDraft>
            {
                TestingUtility.GetPriceDraft(20),
                TestingUtility.GetPriceDraft(30, country: "DE")
            };
            var prices2 = new List<PriceDraft>
            {
                TestingUtility.GetPriceDraft(30),
                TestingUtility.GetPriceDraft(40, country: "DE")
            };
            var product1 = this.productProjectionsFixture.CreateCustomizedProduct(prices: prices1);
            var product2 = this.productProjectionsFixture.CreateCustomizedProduct(prices: prices2);

            string[] productIds = new string[]
            {
                product1.Id,
                product2.Id
            };

            //Act
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.SetPriceCountry("DE");
            searchRequest.SetPriceCurrency("EUR");
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.FilterQuery(p => p.Variants.Any(v => v.ScopedPrice.Value.CentAmount.Range(null, 30)));

            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();
                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Equal(product1.Id, searchResults.Results[0].Id);
            });
        }

        [Fact]
        public void FilterByScopedPriceCurrentValue()
        {
            //Arrange
            var prices1 = new List<PriceDraft>
            {
                TestingUtility.GetPriceDraft(20),
                TestingUtility.GetPriceDraft(30, country: "DE")
            };
            var product1 = this.productProjectionsFixture.CreateCustomizedProduct(prices: prices1);
            var productDiscount = this.productProjectionsFixture.CreateProductDiscountOfAbsoluteValue(product1.Id, 5);


            //Act
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.SetPriceCountry("DE");
            searchRequest.SetPriceCurrency("EUR");
            searchRequest.FilterQuery(p => p.Id == product1.Id.valueOf());
            searchRequest.FilterQuery(p => p.Variants.Any(v => v.ScopedPrice.CurrentValue.CentAmount.Range(null, 25)));

            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();
                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Equal(product1.Id, searchResults.Results[0].Id);

                var masterVariant = searchResults.Results[0].MasterVariant;
                Assert.True(masterVariant.ScopedPriceDiscounted);
                Assert.NotNull(masterVariant.ScopedPrice);
                var scopedPrice = masterVariant.ScopedPrice;
                Assert.NotNull(scopedPrice.Discounted);
                Assert.Equal(30, scopedPrice.Value.CentAmount);
                Assert.Equal(25, scopedPrice.Discounted.Value.CentAmount);
                Assert.Equal(productDiscount.Id, scopedPrice.Discounted.Discount.Id);

                Assert.NotNull(masterVariant.Price);
                Assert.Equal(30, masterVariant.Price.Value.CentAmount);
                Assert.Equal(25, masterVariant.Price.Discounted.Value.CentAmount);
                Assert.Equal(productDiscount.Id, masterVariant.Price.Discounted.Discount.Id);
            });
        }

        [Fact]
        public void FilterByScopedPriceDiscountedValue()
        {
            //Arrange
            var prices1 = new List<PriceDraft>
            {
                TestingUtility.GetPriceDraft(20),
                TestingUtility.GetPriceDraft(30, country: "DE")
            };
            var product1 = this.productProjectionsFixture.CreateCustomizedProduct(prices: prices1);
            var productDiscount = this.productProjectionsFixture.CreateProductDiscountOfAbsoluteValue(product1.Id, 5);


            //Act
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.SetPriceCountry("DE");
            searchRequest.SetPriceCurrency("EUR");
            searchRequest.FilterQuery(p => p.Id == product1.Id.valueOf());
            searchRequest.FilterQuery(p =>
                p.Variants.Any(v => v.ScopedPrice.Discounted.Value.CentAmount.Range(null, 25)));

            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();
                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Equal(product1.Id, searchResults.Results[0].Id);

                var masterVariant = searchResults.Results[0].MasterVariant;
                Assert.True(masterVariant.ScopedPriceDiscounted);
                Assert.NotNull(masterVariant.ScopedPrice);
                var scopedPrice = masterVariant.ScopedPrice;
                Assert.NotNull(scopedPrice.Discounted);
                Assert.Equal(30, scopedPrice.Value.CentAmount);
                Assert.Equal(25, scopedPrice.Discounted.Value.CentAmount);
                Assert.Equal(productDiscount.Id, scopedPrice.Discounted.Discount.Id);

                Assert.NotNull(masterVariant.Price);
                Assert.Equal(30, masterVariant.Price.Value.CentAmount);
                Assert.Equal(25, masterVariant.Price.Discounted.Value.CentAmount);
                Assert.Equal(productDiscount.Id, masterVariant.Price.Discounted.Discount.Id);
            });
        }

        [Fact]
        public void FilterByScopedPriceDiscounted()
        {
            //Arrange
            var prices1 = new List<PriceDraft>
            {
                TestingUtility.GetPriceDraft(20),
                TestingUtility.GetPriceDraft(30, country: "DE")
            };
            var product1 = this.productProjectionsFixture.CreateCustomizedProduct(prices: prices1);
            var productDiscount = this.productProjectionsFixture.CreateProductDiscountOfAbsoluteValue(product1.Id, 5);


            //Act
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.SetPriceCountry("DE");
            searchRequest.SetPriceCurrency("EUR");
            searchRequest.FilterQuery(p => p.Id == product1.Id.valueOf());
            searchRequest.FilterQuery(p => p.Variants.Any(v => v.ScopedPriceDiscounted == true));

            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();
                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Equal(product1.Id, searchResults.Results[0].Id);

                var masterVariant = searchResults.Results[0].MasterVariant;
                Assert.True(masterVariant.ScopedPriceDiscounted);
                Assert.NotNull(masterVariant.ScopedPrice);
                var scopedPrice = masterVariant.ScopedPrice;
                Assert.NotNull(scopedPrice.Discounted);
                Assert.Equal(30, scopedPrice.Value.CentAmount);
                Assert.Equal(25, scopedPrice.Discounted.Value.CentAmount);
                Assert.Equal(productDiscount.Id, scopedPrice.Discounted.Discount.Id);

                Assert.NotNull(masterVariant.Price);
                Assert.Equal(30, masterVariant.Price.Value.CentAmount);
                Assert.Equal(25, masterVariant.Price.Discounted.Value.CentAmount);
                Assert.Equal(productDiscount.Id, masterVariant.Price.Discounted.Discount.Id);
            });
        }


        [Fact]
        public void FilterBySpecificSku()
        {
            //Arrange
            var sku = TestingUtility.RandomString(10);
            var productWithSku = this.productProjectionsFixture.CreateCustomizedProduct(sku);

            //Act (filter by specific sku)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Variants.Any(v => v.Sku == sku));

            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();
                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == productWithSku.Id);
            });
        }

        [Fact]
        public void FilterBySkuMissing()
        {
            //Arrange
            var sku = TestingUtility.RandomString(10);
            var productWithSku = this.productProjectionsFixture.CreateCustomizedProduct(sku);
            var productWithoutSku = this.productProjectionsFixture.CreateCustomizedProduct(null);

            string[] productIds = new string[]
            {
                productWithSku.Id,
                productWithoutSku.Id
            };

            //Act (filter only products with no sku set)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.FilterQuery(p => p.Variants.Any(v => v.Sku.Missing()));

            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();
                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == productWithoutSku.Id);
            });
        }

        [Fact]
        public void FilterBySkuExists()
        {
            //Arrange
            var sku = TestingUtility.RandomString(10);
            var productWithSku = this.productProjectionsFixture.CreateCustomizedProduct(sku);
            var productWithoutSku = this.productProjectionsFixture.CreateCustomizedProduct(null);

            string[] productIds = new string[]
            {
                productWithSku.Id,
                productWithoutSku.Id
            };

            //Act (filter only products with a sku set)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.FilterQuery(p => p.Variants.Any(v => v.Sku.Exists()));

            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();
                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == productWithSku.Id);
            });
        }


        [Fact]
        public void FilterBySpecificProductKey()
        {
            //Arrange
            var key = TestingUtility.RandomString(10);
            var productWithKey = this.productProjectionsFixture.CreateCustomizedProduct(productKey: key);

            //Act (filter by specific key)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Key == key);

            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();
                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == productWithKey.Id);
            });
        }

        [Fact]
        public void FilterByProductKeyMissing()
        {
            //Arrange
            var key = TestingUtility.RandomString(10);
            var productWithKey = this.productProjectionsFixture.CreateCustomizedProduct(productKey: key);
            var productWithoutKey = this.productProjectionsFixture.CreateCustomizedProduct(productKey: null);

            string[] productIds = new string[]
            {
                productWithKey.Id,
                productWithoutKey.Id
            };

            //Act (filter only products with no key set)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.FilterQuery(p => p.Key.Missing());

            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();
                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == productWithoutKey.Id);
            });
        }

        [Fact]
        public void FilterByProductKeyExists()
        {
            //Arrange
            var key = TestingUtility.RandomString(10);
            var productWithKey = this.productProjectionsFixture.CreateCustomizedProduct(productKey: key);
            var productWithoutKey = this.productProjectionsFixture.CreateCustomizedProduct(productKey: null);

            string[] productIds = new string[]
            {
                productWithKey.Id,
                productWithoutKey.Id
            };

            //Act (filter only products with a key set)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.FilterQuery(p => p.Key.Exists());

            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();
                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == productWithKey.Id);
            });
        }


        [Fact]
        public void FilterByProductVariantKey()
        {
            //Arrange
            var key = TestingUtility.RandomString(10);
            var productWithKey = this.productProjectionsFixture.CreateCustomizedProduct(productVariantKey: key);

            //Act (filter by specific variant key)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Variants.Any(v => v.Key == key));

            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();
                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == productWithKey.Id);
            });
        }

        [Fact]
        public void FilterByProductVariantKeyMissing()
        {
            //Arrange
            var key = TestingUtility.RandomString(10);
            var productWithKey = this.productProjectionsFixture.CreateCustomizedProduct(productVariantKey: key);
            var productWithoutKey = this.productProjectionsFixture.CreateCustomizedProduct(productVariantKey: null);

            string[] productIds = new string[]
            {
                productWithKey.Id,
                productWithoutKey.Id
            };

            //Act (filter only products that have a ProductVariant with no key set)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.FilterQuery(p => p.Variants.Any(v => v.Key.Missing()));

            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();
                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == productWithoutKey.Id);
            });
        }

        [Fact]
        public void FilterByProductVariantKeyExists()
        {
            //Arrange
            var key = TestingUtility.RandomString(10);
            var productWithKey = this.productProjectionsFixture.CreateCustomizedProduct(productVariantKey: key);
            var productWithoutKey = this.productProjectionsFixture.CreateCustomizedProduct(productVariantKey: null);

            string[] productIds = new string[]
            {
                productWithKey.Id,
                productWithoutKey.Id
            };

            //Act (filter only products that have a ProductVariant with key set)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.FilterQuery(p => p.Variants.Any(v => v.Key.Exists()));

            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();
                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == productWithKey.Id);
            });
        }


        [Fact]
        public void FilterByProductTypeWithReferenceExpansion()
        {
            //Arrange
            var productType1 = this.productProjectionsFixture.CreateNewProductType();
            var product1 = this.productProjectionsFixture.CreateProduct(productType: productType1);


            //Act (filter only the product which matches the specified ProductType)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.ProductType.Id == productType1.Id.valueOf());
            searchRequest.Expand(p => p.ProductType);

            var searchRequestUsingFilter = new SearchProductProjectionsCommand();
            searchRequestUsingFilter.SetStaged(true);
            searchRequestUsingFilter.Filter(p => p.ProductType.Id == productType1.Id.valueOf());

            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();

                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == product1.Id);
                Assert.NotNull(searchResults.Results[0].ProductType.Obj);
                Assert.Equal(productType1.Key, searchResults.Results[0].ProductType.Obj.Key);


                var searchResultsUsingFilter = commerceToolsClient.ExecuteAsync(searchRequestUsingFilter).Result;
                Assert.Single(searchResultsUsingFilter.Results);
                Assert.Contains(searchResultsUsingFilter.Results, p => p.Id == product1.Id);
            });
        }

        [Fact]
        public void FilterBySpecificTaxCategory()
        {
            //Arrange
            var taxCategory = this.productProjectionsFixture.CreateNewTaxCategory();
            var productWithTaxCategory =
                this.productProjectionsFixture.CreateCustomizedProduct(taxCategory: taxCategory);

            //Act (filter only the product which matches the specified TaxCategory)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.TaxCategory.Id == taxCategory.Id.valueOf());

            var searchRequestUsingFilter = new SearchProductProjectionsCommand();
            searchRequestUsingFilter.SetStaged(true);
            searchRequestUsingFilter.Filter(p => p.TaxCategory.Id == taxCategory.Id.valueOf());

            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();

                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == productWithTaxCategory.Id);

                var searchResultsUsingFilter = commerceToolsClient.ExecuteAsync(searchRequestUsingFilter).Result;
                Assert.Single(searchResultsUsingFilter.Results);
                Assert.Contains(searchResultsUsingFilter.Results, p => p.Id == productWithTaxCategory.Id);
            });
        }

        [Fact]
        public void FilterByTaxCategoryMissing()
        {
            //Arrange
            var taxCategory = this.productProjectionsFixture.CreateNewTaxCategory();
            var productWithTaxCategory =
                this.productProjectionsFixture.CreateCustomizedProduct(taxCategory: taxCategory);
            var productWithoutTaxCategory = this.productProjectionsFixture.CreateCustomizedProduct(taxCategory: null);

            //Act (filter only the product which have TaxCategory not set)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.TaxCategory.Missing());

            var searchRequestUsingFilter = new SearchProductProjectionsCommand();
            searchRequestUsingFilter.SetStaged(true);
            searchRequestUsingFilter.Filter(p => p.TaxCategory.Missing());

            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();

                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == productWithoutTaxCategory.Id);

                var searchResultsUsingFilter = commerceToolsClient.ExecuteAsync(searchRequestUsingFilter).Result;
                Assert.Single(searchResultsUsingFilter.Results);
                Assert.Contains(searchResultsUsingFilter.Results, p => p.Id == productWithoutTaxCategory.Id);
            });
        }

        [Fact]
        public void FilterByTaxCategoryExists()
        {
            //Arrange
            var taxCategory = this.productProjectionsFixture.CreateNewTaxCategory();
            var productWithTaxCategory =
                this.productProjectionsFixture.CreateCustomizedProduct(taxCategory: taxCategory);
            var productWithoutTaxCategory = this.productProjectionsFixture.CreateCustomizedProduct(taxCategory: null);

            //Act (filter only the product which have TaxCategory not set)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.TaxCategory.Exists());

            var searchRequestUsingFilter = new SearchProductProjectionsCommand();
            searchRequestUsingFilter.SetStaged(true);
            searchRequestUsingFilter.Filter(p => p.TaxCategory.Exists());

            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();

                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == productWithTaxCategory.Id);

                var searchResultsUsingFilter = commerceToolsClient.ExecuteAsync(searchRequestUsingFilter).Result;
                Assert.Single(searchResultsUsingFilter.Results);
                Assert.Contains(searchResultsUsingFilter.Results, p => p.Id == productWithTaxCategory.Id);
            });
        }


        [Fact]
        public void FilterBySpecificState()
        {
            //Arrange
            var state = this.productProjectionsFixture.CreateNewState();
            var productWithState = this.productProjectionsFixture.CreateCustomizedProduct(state: state);

            //Act (filter only the product which matches the specified state)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.State.Id == state.Id.valueOf());

            var searchRequestUsingFilter = new SearchProductProjectionsCommand();
            searchRequestUsingFilter.SetStaged(true);
            searchRequestUsingFilter.Filter(p => p.State.Id == state.Id.valueOf());

            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();

                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == productWithState.Id);

                var searchResultsUsingFilter = commerceToolsClient.ExecuteAsync(searchRequestUsingFilter).Result;
                Assert.Single(searchResultsUsingFilter.Results);
                Assert.Contains(searchResultsUsingFilter.Results, p => p.Id == productWithState.Id);
            });
        }

        [Fact]
        public void FilterByStateMissing()
        {
            //Arrange
            var state = this.productProjectionsFixture.CreateNewState();
            var productWithState = this.productProjectionsFixture.CreateCustomizedProduct(state: state);
            var productWithoutState = this.productProjectionsFixture.CreateCustomizedProduct(state: null);

            //Act (filter only the product which matches the specified state)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.State.Missing());

            var searchRequestUsingFilter = new SearchProductProjectionsCommand();
            searchRequestUsingFilter.SetStaged(true);
            searchRequestUsingFilter.Filter(p => p.State.Missing());

            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();

                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == productWithoutState.Id);

                var searchResultsUsingFilter = commerceToolsClient.ExecuteAsync(searchRequestUsingFilter).Result;
                Assert.Single(searchResultsUsingFilter.Results);
                Assert.Contains(searchResultsUsingFilter.Results, p => p.Id == productWithoutState.Id);
            });
        }

        [Fact]
        public void FilterByStateExists()
        {
            //Arrange
            var state = this.productProjectionsFixture.CreateNewState();
            var productWithState = this.productProjectionsFixture.CreateCustomizedProduct(state: state);
            var productWithoutState = this.productProjectionsFixture.CreateCustomizedProduct(state: null);

            //Act (filter only the product which matches the specified state)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.State.Exists());

            var searchRequestUsingFilter = new SearchProductProjectionsCommand();
            searchRequestUsingFilter.SetStaged(true);
            searchRequestUsingFilter.Filter(p => p.State.Exists());

            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();

                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == productWithState.Id);

                var searchResultsUsingFilter = commerceToolsClient.ExecuteAsync(searchRequestUsingFilter).Result;
                Assert.Single(searchResultsUsingFilter.Results);
                Assert.Contains(searchResultsUsingFilter.Results, p => p.Id == productWithState.Id);
            });
        }


        [Fact]
        public void FilterByReviewRatingStatistics()
        {
            //Arrange
            var product =
                this.productProjectionsFixture.CreateCustomizedProduct(productKey: TestingUtility.RandomString(10));
            var review1 = this.productProjectionsFixture.CreateProductReview(product.Key, 1);
            var review2 = this.productProjectionsFixture.CreateProductReview(product.Key, 3);

            //Act
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.ReviewRatingStatistics.AverageRating == 2D);
            searchRequest.FilterQuery(p => p.ReviewRatingStatistics.LowestRating == 1);
            searchRequest.FilterQuery(p => p.ReviewRatingStatistics.HighestRating == 3);
            searchRequest.FilterQuery(p => p.ReviewRatingStatistics.Count == 2);
            searchRequest.FilterQuery(p => p.Id == product.Id.valueOf());

            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();

                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == product.Id);
            });
        }


        [Fact]
        public void FilterByCustomAttributeValue()
        {
            //Arrange
            var product10 = this.productProjectionsFixture.CreateCustomizedProduct(
                attributes: new List<Attribute> {new NumberAttribute {Name = "number-attribute-name", Value = 10}});
            var product30 = this.productProjectionsFixture.CreateCustomizedProduct(
                attributes: new List<Attribute> {new NumberAttribute {Name = "number-attribute-name", Value = 30}});
            var product50 = this.productProjectionsFixture.CreateCustomizedProduct(
                attributes: new List<Attribute> {new NumberAttribute {Name = "number-attribute-name", Value = 50}});

            var productIds = new[] {product10.Id, product30.Id, product50.Id};

            //Act (filter only the product which have custom attribute value)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.FilterQuery(p => p.Variants.Any(v =>
                v.Attributes.Any(a => a.Name == "number-attribute-name" && ((NumberAttribute) a).Value == 10)));

            var searchRequestByRange = new SearchProductProjectionsCommand();
            searchRequestByRange.SetStaged(true);
            searchRequestByRange.FilterQuery(p => p.Id.In(productIds));
            searchRequestByRange.FilterQuery(p => p.Variants.Any(v =>
                v.Attributes.Any(a => a.Name == "number-attribute-name" && ((NumberAttribute) a).Value.Range(20, 60))));

            var searchRequestByRangeTo = new SearchProductProjectionsCommand();
            searchRequestByRangeTo.SetStaged(true);
            searchRequestByRangeTo.FilterQuery(p => p.Id.In(productIds));
            searchRequestByRangeTo.FilterQuery(p => p.Variants.Any(v =>
                v.Attributes.Any(a =>
                    a.Name == "number-attribute-name" && ((NumberAttribute) a).Value.Range(null, 40))));


            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();

                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == product10.Id);

                var searchResultsByRange = commerceToolsClient.ExecuteAsync(searchRequestByRange).Result;
                Assert.Equal(2, searchResultsByRange.Results.Count);
                Assert.Contains(searchResultsByRange.Results, p => p.Id == product30.Id);
                Assert.Contains(searchResultsByRange.Results, p => p.Id == product50.Id);

                var searchResultsByRangeTo = commerceToolsClient.ExecuteAsync(searchRequestByRangeTo).Result;
                Assert.Equal(2, searchResultsByRangeTo.Results.Count);
                Assert.Contains(searchResultsByRangeTo.Results, p => p.Id == product10.Id);
                Assert.Contains(searchResultsByRangeTo.Results, p => p.Id == product30.Id);
            });
        }

        [Fact]
        public void FilterByCustomAttributeMissing()
        {
            //Arrange
            var productWithCustomAttribute = this.productProjectionsFixture.CreateCustomizedProduct(
                attributes: new List<Attribute> {new NumberAttribute {Name = "number-attribute-name", Value = 10}});
            var productWithoutCustomAttribute =
                this.productProjectionsFixture.CreateCustomizedProduct(attributes: null);

            string[] productIds = new[]
            {
                productWithCustomAttribute.Id,
                productWithoutCustomAttribute.Id
            };

            //Act (filter only products that don't have custom attribute set)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.FilterQuery(p =>
                p.Variants.Any(v => v.Attributes.Any(a => a.Name == "number-attribute-name")).Missing());

            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();
                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == productWithoutCustomAttribute.Id);
            });
        }

        [Fact]
        public void FilterByCustomAttributeExists()
        {
            //Arrange
            var productWithCustomAttribute = this.productProjectionsFixture.CreateCustomizedProduct(
                attributes: new List<Attribute> {new NumberAttribute {Name = "number-attribute-name", Value = 10}});
            var productWithoutCustomAttribute =
                this.productProjectionsFixture.CreateCustomizedProduct(attributes: null);

            string[] productIds = new[]
            {
                productWithCustomAttribute.Id,
                productWithoutCustomAttribute.Id
            };

            //Act (filter only products that have custom attribute set)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.FilterQuery(p =>
                p.Variants.Any(v => v.Attributes.Any(a => a.Name == "number-attribute-name")).Exists());

            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();
                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == productWithCustomAttribute.Id);
            });
        }

        [Fact]
        public void FilterByEnumCustomAttribute()
        {
            //Arrange
            var productWithEnum1 = this.productProjectionsFixture.CreateCustomizedProduct(
                attributes: new List<Attribute>
                {
                    new EnumAttribute
                    {
                        Name = "enum-attribute-name", Value = new PlainEnumValue
                        {
                            Key = "enum-key-1",
                            Label = "enum-label-1"
                        }
                    }
                });
            var productWithEnum2 = this.productProjectionsFixture.CreateCustomizedProduct(
                attributes: new List<Attribute>
                {
                    new EnumAttribute
                    {
                        Name = "enum-attribute-name", Value = new PlainEnumValue
                        {
                            Key = "enum-key-2",
                            Label = "enum-label-2"
                        }
                    }
                });

            string[] productIds = new[]
            {
                productWithEnum1.Id,
                productWithEnum2.Id
            };

            //Act (filter only products that have enum custom attribute with specific value )
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.FilterQuery(p => p.Variants.Any(v =>
                v.Attributes.Any(a =>
                    a.Name == "enum-attribute-name" && ((EnumAttribute) a).Value.Key == "enum-key-1")));

            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();
                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == productWithEnum1.Id);
            });
        }


        [Fact]
        public void FilterByMoneyCustomAttributeValue()
        {
            //Arrange
            var money30 = new Money {CentAmount = 30, CurrencyCode = "EUR"};
            var money50 = new Money {CentAmount = 50, CurrencyCode = "EUR"};
            var money70 = new Money {CentAmount = 70, CurrencyCode = "USD"};

            var product30 = this.productProjectionsFixture.CreateCustomizedProduct(
                attributes: new List<Attribute> {new MoneyAttribute {Name = "money-attribute-name", Value = money30}});
            var product50 = this.productProjectionsFixture.CreateCustomizedProduct(
                attributes: new List<Attribute> {new MoneyAttribute {Name = "money-attribute-name", Value = money50}});
            var product70 = this.productProjectionsFixture.CreateCustomizedProduct(
                attributes: new List<Attribute> {new MoneyAttribute {Name = "money-attribute-name", Value = money70}});


            var productIds = new[] {product30.Id, product50.Id, product70.Id};

            //Act (filter only the product which have money custom attribute with specific value)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.FilterQuery(p => p.Variants.Any(v => v.Attributes.Any(a =>
                a.Name == "money-attribute-name" &&
                ((MoneyAttribute) a).Value.CentAmount == money30.CentAmount.valueOf())));

            var searchByCurrencyRequest = new SearchProductProjectionsCommand();
            searchByCurrencyRequest.SetStaged(true);
            searchByCurrencyRequest.FilterQuery(p => p.Id.In(productIds));
            searchByCurrencyRequest.FilterQuery(p => p.Variants.Any(v => v.Attributes.Any(a =>
                a.Name == "money-attribute-name" &&
                ((MoneyAttribute) a).Value.CurrencyCode == money70.CurrencyCode.valueOf())));

            var searchByRangeRequest = new SearchProductProjectionsCommand();
            searchByRangeRequest.SetStaged(true);
            searchByRangeRequest.FilterQuery(p => p.Id.In(productIds));
            searchByRangeRequest.FilterQuery(p => p.Variants.Any(v => v.Attributes.Any(a =>
                a.Name == "money-attribute-name" && ((MoneyAttribute) a).Value.CentAmount.Range(10, 40))));


            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();

                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == product30.Id);

                var searchByCurrencyResults = commerceToolsClient.ExecuteAsync(searchByCurrencyRequest).Result;
                Assert.Single(searchByCurrencyResults.Results);
                Assert.Contains(searchByCurrencyResults.Results, p => p.Id == product70.Id);

                var searchByRangeResults = commerceToolsClient.ExecuteAsync(searchByRangeRequest).Result;
                Assert.Single(searchByRangeResults.Results);
                Assert.Contains(searchByRangeResults.Results, p => p.Id == product30.Id);
            });
        }


        [Fact]
        public void FilterByReferenceCustomAttribute()
        {
            //Arrange
            var productType1 = this.productProjectionsFixture.CreateNewProductType();
            var productType2 = this.productProjectionsFixture.CreateNewProductType();

            var productOfType1 = this.productProjectionsFixture.CreateCustomizedProduct(
                attributes: new List<Attribute>
                {
                    new ReferenceAttribute
                    {
                        Name = "reference-attribute-name", Value = new Reference
                        {
                            Id = productType1.Id,
                            TypeId = ReferenceTypeId.ProductType
                        }
                    }
                });

            var productOfType2 = this.productProjectionsFixture.CreateCustomizedProduct(
                attributes: new List<Attribute>
                {
                    new ReferenceAttribute
                    {
                        Name = "reference-attribute-name", Value = new Reference
                        {
                            Id = productType2.Id,
                            TypeId = ReferenceTypeId.ProductType
                        }
                    }
                });


            string[] productIds = new[]
            {
                productOfType1.Id,
                productOfType2.Id
            };

            //Act
            var searchRequestByReferenceTypeId = new SearchProductProjectionsCommand();
            searchRequestByReferenceTypeId.SetStaged(true);
            searchRequestByReferenceTypeId.FilterQuery(p => p.Id.In(productIds));
            searchRequestByReferenceTypeId.FilterQuery(p => p.Variants.Any(v => v.Attributes.Any(a =>
                a.Name == "reference-attribute-name" && ((ReferenceAttribute) a).Value.TypeId.Value.ToString() ==
                ReferenceTypeId.ProductType.GetDescription().valueOf())));

            var searchRequestByTypeId = new SearchProductProjectionsCommand();
            searchRequestByTypeId.SetStaged(true);
            searchRequestByTypeId.FilterQuery(p => p.Id.In(productIds));
            searchRequestByTypeId.FilterQuery(p => p.Variants.Any(v => v.Attributes.Any(a =>
                a.Name == "reference-attribute-name" &&
                ((ReferenceAttribute) a).Value.Id == productType1.Id.valueOf())));

            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();

                var searchByReferenceTypeIdResults =
                    commerceToolsClient.ExecuteAsync(searchRequestByReferenceTypeId).Result;
                Assert.Equal(2, searchByReferenceTypeIdResults.Results.Count);
                Assert.Contains(searchByReferenceTypeIdResults.Results, p => p.Id == productOfType1.Id);
                Assert.Contains(searchByReferenceTypeIdResults.Results, p => p.Id == productOfType2.Id);

                var searchByProductTypeIdResults = commerceToolsClient.ExecuteAsync(searchRequestByTypeId).Result;
                Assert.Single(searchByProductTypeIdResults.Results);
                Assert.Contains(searchByProductTypeIdResults.Results, p => p.Id == productOfType1.Id);
            });
        }

        [Fact]
        public void FilterByProductAvailability()
        {
            //Arrange
            var productNotAvailableInStock = this.productProjectionsFixture.CreateCustomizedProduct(
                sku: TestingUtility.RandomString(10),
                productVariantAvailability: new ProductVariantAvailability
                {
                    AvailableQuantity = 0
                });

            var productAvailableWithQuantity3 = this.productProjectionsFixture.CreateCustomizedProduct(
                sku: TestingUtility.RandomString(10),
                productVariantAvailability: new ProductVariantAvailability
                {
                    AvailableQuantity = 3
                });

            var productAvailableWithQuantity6 = this.productProjectionsFixture.CreateCustomizedProduct(
                sku: TestingUtility.RandomString(10),
                productVariantAvailability: new ProductVariantAvailability
                {
                    AvailableQuantity = 6
                });

            string[] productIds = new[]
            {
                productNotAvailableInStock.Id,
                productAvailableWithQuantity3.Id,
                productAvailableWithQuantity6.Id
            };

            //Act
            var searchProductsOnStock = new SearchProductProjectionsCommand();
            searchProductsOnStock.SetStaged(true);
            searchProductsOnStock.FilterQuery(p => p.Id.In(productIds));
            searchProductsOnStock.FilterQuery(p => p.Variants.Any(v => v.Availability.IsOnStock == true));

            var searchProductsByAvailability = new SearchProductProjectionsCommand();
            searchProductsByAvailability.SetStaged(true);
            searchProductsByAvailability.FilterQuery(p => p.Id.In(productIds));
            searchProductsByAvailability.FilterQuery(p =>
                p.Variants.Any(v => v.Availability.AvailableQuantity.Range(4, 6)));
            searchProductsByAvailability.RangeFacet(p =>
                p.Variants.Any(v => v.Availability.AvailableQuantity.Range(1, null)));

            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();

                var searchProductsOnStockResult = commerceToolsClient.ExecuteAsync(searchProductsOnStock).Result;
                Assert.Equal(2, searchProductsOnStockResult.Results.Count);
                Assert.DoesNotContain(searchProductsOnStockResult.Results, p => p.Id == productNotAvailableInStock.Id);
                Assert.Contains(searchProductsOnStockResult.Results, p => p.Id == productAvailableWithQuantity3.Id);
                Assert.Contains(searchProductsOnStockResult.Results, p => p.Id == productAvailableWithQuantity6.Id);


                var searchProductsByAvailabilityResult =
                    commerceToolsClient.ExecuteAsync(searchProductsByAvailability).Result;
                Assert.Single(searchProductsByAvailabilityResult.Results);
                Assert.Equal(productAvailableWithQuantity6.Id, searchProductsByAvailabilityResult.Results[0].Id);

                Assert.Single(searchProductsByAvailabilityResult.Facets);
                var facetResult = searchProductsByAvailabilityResult.Facets.First().Value as RangeFacetResult;

                Assert.NotNull(facetResult);
                Assert.Single(facetResult.Ranges);
                Assert.Equal(6, facetResult.Ranges[0].Max);
            });
        }

        [Fact]
        public void FilterProductsOnStockWithChannel()
        {
            //Arrange
            var channel = this.productProjectionsFixture.CreateNewChannel(ChannelRole.InventorySupply);
            var productAvailableInChannel = this.productProjectionsFixture.CreateCustomizedProduct(
                sku: TestingUtility.RandomString(10),
                productVariantAvailability: new ProductVariantAvailability
                {
                    AvailableQuantity = 2
                }, supplyChannel: channel);
            var productNotAvailableInChannel = this.productProjectionsFixture.CreateCustomizedProduct(
                sku: TestingUtility.RandomString(10),
                productVariantAvailability: new ProductVariantAvailability
                {
                    AvailableQuantity = 0
                }, supplyChannel: channel);

            string[] productIds = new[]
            {
                productAvailableInChannel.Id,
                productNotAvailableInChannel.Id
            };

            //Act
            var searchProductsOnStock = new SearchProductProjectionsCommand();
            searchProductsOnStock.SetStaged(true);
            searchProductsOnStock.FilterQuery(p => p.Id.In(productIds));
            searchProductsOnStock.FilterQuery(p =>
                p.Variants.Any(v => v.Availability.Channels[channel.Id.valueOf()].IsOnStock == true));

            var searchProductsNotAvailableOnStock = new SearchProductProjectionsCommand();
            searchProductsNotAvailableOnStock.SetStaged(true);
            searchProductsNotAvailableOnStock.FilterQuery(p => p.Id.In(productIds));
            searchProductsNotAvailableOnStock.FilterQuery(p =>
                p.Variants.Any(v => v.Availability.Channels[channel.Id.valueOf()].IsOnStock == false));

            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();

                var searchProductsOnStockResult = commerceToolsClient.ExecuteAsync(searchProductsOnStock).Result;
                Assert.Single(searchProductsOnStockResult.Results);
                Assert.Equal(productAvailableInChannel.Id, searchProductsOnStockResult.Results[0].Id);

                var searchProductsNotAvailableOnStockResult =
                    commerceToolsClient.ExecuteAsync(searchProductsNotAvailableOnStock).Result;
                Assert.Single(searchProductsNotAvailableOnStockResult.Results);
                Assert.Equal(productNotAvailableInChannel.Id, searchProductsNotAvailableOnStockResult.Results[0].Id);
            });
        }

        [Fact]
        public void FilterProductsByAvailabilityRangeWithChannel()
        {
            //Arrange
            var channel = this.productProjectionsFixture.CreateNewChannel(ChannelRole.InventorySupply);

            var productAvailableWithQuantity5 = this.productProjectionsFixture.CreateCustomizedProduct(
                sku: TestingUtility.RandomString(10),
                productVariantAvailability: new ProductVariantAvailability
                {
                    AvailableQuantity = 5
                }, supplyChannel: channel);

            var productAvailableWithQuantity10 = this.productProjectionsFixture.CreateCustomizedProduct(
                sku: TestingUtility.RandomString(10),
                productVariantAvailability: new ProductVariantAvailability
                {
                    AvailableQuantity = 10
                }, supplyChannel: channel);

            string[] productIds = new[]
            {
                productAvailableWithQuantity5.Id,
                productAvailableWithQuantity10.Id
            };

            //Act
            var searchProductsByRange = new SearchProductProjectionsCommand();
            searchProductsByRange.SetStaged(true);
            searchProductsByRange.FilterQuery(p => p.Id.In(productIds));
            searchProductsByRange.FilterQuery(p =>
                p.Variants.Any(v => v.Availability.Channels[channel.Id.valueOf()].AvailableQuantity.Range(6, 10)));


            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();

                var searchProductsByRangeResult = commerceToolsClient.ExecuteAsync(searchProductsByRange).Result;
                Assert.Single(searchProductsByRangeResult.Results);
                Assert.Equal(productAvailableWithQuantity10.Id, searchProductsByRangeResult.Results[0].Id);
            });
        }

        [Fact]
        public void FilterProductsOnStockWithSpecificChannels()
        {
            //Arrange
            var channel1 = this.productProjectionsFixture.CreateNewChannel(ChannelRole.InventorySupply);
            var channel2 = this.productProjectionsFixture.CreateNewChannel(ChannelRole.InventorySupply);
            var channel3 = this.productProjectionsFixture.CreateNewChannel(ChannelRole.InventorySupply);

            var productAvailableInChannel1 = this.productProjectionsFixture.CreateCustomizedProduct(
                sku: TestingUtility.RandomString(10),
                productVariantAvailability: new ProductVariantAvailability
                {
                    AvailableQuantity = 2
                }, supplyChannel: channel1);

            var productAvailableInChannel2 = this.productProjectionsFixture.CreateCustomizedProduct(
                sku: TestingUtility.RandomString(10),
                productVariantAvailability: new ProductVariantAvailability
                {
                    AvailableQuantity = 5
                }, supplyChannel: channel2);

            var productAvailableInChannel3 = this.productProjectionsFixture.CreateCustomizedProduct(
                sku: TestingUtility.RandomString(10),
                productVariantAvailability: new ProductVariantAvailability
                {
                    AvailableQuantity = 3
                }, supplyChannel: channel3);

            string[] productIds = new[]
            {
                productAvailableInChannel1.Id,
                productAvailableInChannel2.Id,
                productAvailableInChannel3.Id
            };

            //Act
            var searchProductsOnStockInSpecificChannels = new SearchProductProjectionsCommand();
            searchProductsOnStockInSpecificChannels.SetStaged(true);
            searchProductsOnStockInSpecificChannels.FilterQuery(p => p.Id.In(productIds));
            searchProductsOnStockInSpecificChannels.FilterQuery(p =>
                p.Variants.Any(v => v.Availability.IsOnStockInChannels(channel1.Id.valueOf(), channel2.Id.valueOf())));

            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();

                var searchProductsOnStockResult =
                    commerceToolsClient.ExecuteAsync(searchProductsOnStockInSpecificChannels).Result;
                Assert.Equal(2, searchProductsOnStockResult.Results.Count);

                Assert.DoesNotContain(searchProductsOnStockResult.Results, p => p.Id == productAvailableInChannel3.Id);
                Assert.Contains(searchProductsOnStockResult.Results, p => p.Id == productAvailableInChannel1.Id);
                Assert.Contains(searchProductsOnStockResult.Results, p => p.Id == productAvailableInChannel2.Id);
            });
        }


        [Fact]
        public void FilterByProductSearchKeywords()
        {
            //Arrange
            var searchKeywordsList = new List<SearchKeywords>
            {
                new SearchKeywords {Text = "jeans"}
            };
            var searchKeywords = new Dictionary<string, List<SearchKeywords>>
            {
                {
                    "en",
                    searchKeywordsList
                }
            };
            var productWithoutSearchKeywords = this.productProjectionsFixture.CreateCustomizedProduct();
            var productWithSearchKeywords =
                this.productProjectionsFixture.CreateCustomizedProduct(searchKeywords: searchKeywords);

            string[] productIds = new[]
            {
                productWithoutSearchKeywords.Id,
                productWithSearchKeywords.Id
            };

            //Act (filter only the product which matches the specified state)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.FilterQuery(p => p.SearchKeywords["en"].Any(s => s.Text == "jeans"));

            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();

                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == productWithSearchKeywords.Id);
            });
        }

        #endregion


        #region Facets

        [Fact]
        public void TestTermFacetWithProductCategories()
        {
            //Arrange
            var allCategories = this.productProjectionsFixture.SetupCategories();
            Assert.True(allCategories.Count > 0);

            var categoryA = allCategories.FirstOrDefault(c => c.ExternalId == "A");
            var categoryB = allCategories.FirstOrDefault(c => c.ExternalId == "B");

            Assert.NotNull(categoryA);
            Assert.NotNull(categoryB);

            var productA = this.productProjectionsFixture.CreateProduct(productCategory: categoryA);
            var productA1 = this.productProjectionsFixture.CreateProduct(productCategory: categoryA);
            var productB = this.productProjectionsFixture.CreateProduct(productCategory: categoryB);

            var productIds = new[] {productA.Id, productA1.Id, productB.Id};

            //Act
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.TermFacet(p => p.Categories.Select(c => c.Id).FirstOrDefault(), isCountingProducts: true);

            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();
                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Equal(3, searchResults.Results.Count);
                Assert.Single(searchResults.Facets);

                var termFacetResult = searchResults.Facets.FirstOrDefault().Value as TermFacetResult;
                Assert.NotNull(termFacetResult);
                Assert.Equal(2, termFacetResult.Terms.Count);

                Assert.Contains(termFacetResult.Terms,
                    t => t.Term.Equals(categoryA.Id) && t.Count == 2 && t.ProductCount == 2);
                Assert.Contains(termFacetResult.Terms,
                    t => t.Term.Equals(categoryB.Id) && t.Count == 1 && t.ProductCount == 1);
            });
        }

        [Fact]
        public void TestTermFacetNumberAttribute()
        {
            //Arrange
            var value10 = 10.0;
            var value30 = 30.0;

            var product1 = this.productProjectionsFixture.CreateCustomizedProduct(
                attributes: new List<Attribute>
                    {new NumberAttribute {Name = "number-attribute-name", Value = value10}});
            var product2 = this.productProjectionsFixture.CreateCustomizedProduct(
                attributes: new List<Attribute>
                    {new NumberAttribute {Name = "number-attribute-name", Value = value30}});
            var product3 = this.productProjectionsFixture.CreateCustomizedProduct(
                attributes: new List<Attribute>
                    {new NumberAttribute {Name = "number-attribute-name", Value = value30}});

            var productIds = new[] {product1.Id, product2.Id, product3.Id};

            //Act (filter only the product which have number attribute value = 10)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            //filter to the query results after facets have been calculated. so we expect termFacetResult with 2 terms
            searchRequest.Filter(p => p.Variants.Any(v =>
                v.Attributes.Any(a =>
                    a.Name == "number-attribute-name" && ((NumberAttribute) a).Value == value10.valueOf())));

            searchRequest.TermFacet("GroupByNumberAttr", p => p.Variants.Any(v =>
                v.Attributes.Select(a => a.Name == "number-attribute-name").FirstOrDefault()));


            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();

                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == product1.Id);

                Assert.Single(searchResults.Facets);


                var termFacetResult = searchResults.Facets["GroupByNumberAttr"] as TermFacetResult;
                Assert.NotNull(termFacetResult);
                Assert.Equal(2, termFacetResult.Terms.Count);

                Assert.Contains(termFacetResult.Terms, t => t.Term.Equals(value10.ToString("N1")) && t.Count == 1);
                Assert.Contains(termFacetResult.Terms, t => t.Term.Equals(value30.ToString("N1")) && t.Count == 2);
            });
        }

        [Fact]
        public void TestTermFacetCustomMoneyAttributeValue()
        {
            //Arrange
            var money30 = new Money {CentAmount = 30, CurrencyCode = "EUR"};
            var money50 = new Money {CentAmount = 50, CurrencyCode = "EUR"};


            var product1 = this.productProjectionsFixture.CreateCustomizedProduct(
                attributes: new List<Attribute> {new MoneyAttribute {Name = "money-attribute-name", Value = money30}});
            var product2 = this.productProjectionsFixture.CreateCustomizedProduct(
                attributes: new List<Attribute> {new MoneyAttribute {Name = "money-attribute-name", Value = money50}});
            var product3 = this.productProjectionsFixture.CreateCustomizedProduct(
                attributes: new List<Attribute> {new MoneyAttribute {Name = "money-attribute-name", Value = money50}});


            var productIds = new[] {product1.Id, product2.Id, product3.Id};

            //Act (filter only the product which have money custom attribute with specific value money30) and return facets with custom money attribute
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.Filter(p => p.Variants.Any(v => v.Attributes.Any(a =>
                a.Name == "money-attribute-name" &&
                ((MoneyAttribute) a).Value.CentAmount == money30.CentAmount.valueOf())));


            searchRequest.TermFacet("GroupByCustomMoney",
                p => p.Variants.Select(v =>
                        v.Attributes.Where(a => a.Name == "money-attribute-name")
                            .Select(a => ((MoneyAttribute) a).Value.CentAmount.ToString()).FirstOrDefault())
                    .FirstOrDefault(), isCountingProducts: true);

            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();

                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == product1.Id);

                Assert.Single(searchResults.Facets);

                var termFacetResult = searchResults.Facets["GroupByCustomMoney"] as TermFacetResult;
                Assert.NotNull(termFacetResult);
                Assert.Equal(2, termFacetResult.Terms.Count);

                Assert.Contains(termFacetResult.Terms,
                    t => t.Term.Equals(money30.CentAmount.ToString()) && t.Count == 1 && t.ProductCount == 1);
                Assert.Contains(termFacetResult.Terms,
                    t => t.Term.Equals(money50.CentAmount.ToString()) && t.Count == 2 && t.ProductCount == 2);
            });
        }

        [Fact]
        public void TestTermFacetAvailableQuantityInChannels()
        {
            //Arrange
            var quantityProduct2 = 5;
            var quantityProduct3 = 3;

            var channel1 = this.productProjectionsFixture.CreateNewChannel(ChannelRole.InventorySupply);
            var channel2 = this.productProjectionsFixture.CreateNewChannel(ChannelRole.InventorySupply);

            var product1 = this.productProjectionsFixture.CreateCustomizedProduct(
                sku: TestingUtility.RandomString(10),
                productVariantAvailability: new ProductVariantAvailability
                {
                    AvailableQuantity = 2
                }, supplyChannel: channel1);

            var product2 = this.productProjectionsFixture.CreateCustomizedProduct(
                sku: TestingUtility.RandomString(10),
                productVariantAvailability: new ProductVariantAvailability
                {
                    AvailableQuantity = quantityProduct2
                }, supplyChannel: channel2);

            var product3 = this.productProjectionsFixture.CreateCustomizedProduct(
                sku: TestingUtility.RandomString(10),
                productVariantAvailability: new ProductVariantAvailability
                {
                    AvailableQuantity = quantityProduct3
                }, supplyChannel: channel2);

            string[] productIds = new[]
            {
                product1.Id,
                product2.Id,
                product3.Id
            };

            //Act (Filter Products in Channel1) and Return Facets as Counts the ProductVariants for all occurring availableQuantity for the supply channel2
            var searchProductsRequest = new SearchProductProjectionsCommand();
            searchProductsRequest.SetStaged(true);
            searchProductsRequest.FilterQuery(p => p.Id.In(productIds));
            searchProductsRequest.Filter(p =>
                p.Variants.Any(v => v.Availability.IsOnStockInChannels(channel1.Id.valueOf())));

            searchProductsRequest.TermFacet("ProductsAvailabilityInChannel2", p =>
                p.Variants.Select(v => v.Availability.Channels[channel2.Id.valueOf()].AvailableQuantity)
                    .FirstOrDefault(), isCountingProducts: true);

            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();

                var searchProductsResult =
                    commerceToolsClient.ExecuteAsync(searchProductsRequest).Result;
                Assert.Single(searchProductsResult.Results);
                Assert.Contains(searchProductsResult.Results, p => p.Id == product1.Id);

                Assert.Single(searchProductsResult.Facets);

                var termFacetResult = searchProductsResult.Facets["ProductsAvailabilityInChannel2"] as TermFacetResult;
                Assert.NotNull(termFacetResult);

                Assert.Equal(2, termFacetResult.Terms.Count);

                Assert.Contains(termFacetResult.Terms,
                    t => t.Term.Equals(quantityProduct2.ToString()) && t.Count == 1 && t.ProductCount == 1);
                Assert.Contains(termFacetResult.Terms,
                    t => t.Term.Equals(quantityProduct3.ToString()) && t.Count == 1 && t.ProductCount == 1);
            });
        }

        [Fact]
        public void TestRangeFacetByProductAvailability()
        {
            //Arrange
            var productAvailableWithQuantity6 = this.productProjectionsFixture.CreateCustomizedProduct(
                sku: TestingUtility.RandomString(10),
                productVariantAvailability: new ProductVariantAvailability
                {
                    AvailableQuantity = 6
                });

            var productAvailableWithQuantity11 = this.productProjectionsFixture.CreateCustomizedProduct(
                sku: TestingUtility.RandomString(10),
                productVariantAvailability: new ProductVariantAvailability
                {
                    AvailableQuantity = 11
                });

            string[] productIds = new[]
            {
                productAvailableWithQuantity6.Id,
                productAvailableWithQuantity11.Id
            };

            //Act (search Products with quantity >= 10) and return facets with 2 ranges (GreaterThan5,GreaterThan10)

            var searchProductsByAvailability = new SearchProductProjectionsCommand();
            searchProductsByAvailability.SetStaged(true);

            searchProductsByAvailability.FilterQuery(p => p.Id.In(productIds));
            searchProductsByAvailability.Filter(p =>
                p.Variants.Any(v => v.Availability.AvailableQuantity.Range(10, null)));

            searchProductsByAvailability.RangeFacet("GreaterThan5", p =>
                p.Variants.Any(v => v.Availability.AvailableQuantity.Range(5, null)));
            searchProductsByAvailability.RangeFacet("GreaterThan10", p =>
                p.Variants.Any(v => v.Availability.AvailableQuantity.Range(10, null)));

            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();

                var searchProductsByAvailabilityResult =
                    commerceToolsClient.ExecuteAsync(searchProductsByAvailability).Result;
                Assert.Single(searchProductsByAvailabilityResult.Results);
                Assert.Equal(productAvailableWithQuantity11.Id, searchProductsByAvailabilityResult.Results[0].Id);

                Assert.Equal(2, searchProductsByAvailabilityResult.Facets.Count);

                var greaterThan5RangeResult =
                    searchProductsByAvailabilityResult.Facets["GreaterThan5"] as RangeFacetResult;
                var greaterThan10RangeResult =
                    searchProductsByAvailabilityResult.Facets["GreaterThan10"] as RangeFacetResult;

                Assert.NotNull(greaterThan5RangeResult);
                Assert.NotNull(greaterThan10RangeResult);

                Assert.Single(greaterThan5RangeResult.Ranges);
                Assert.Equal(2, greaterThan5RangeResult.Ranges[0].Count);
                Assert.Equal(11, greaterThan5RangeResult.Ranges[0].Max);
                Assert.Equal(6, greaterThan5RangeResult.Ranges[0].Min);
            });
        }

        [Fact]
        public void TestRangeFacetByPriceCentAmount()
        {
            //Arrange
            var euro30 = TestingUtility.GetPriceDraft(30);
            var euro60 = TestingUtility.GetPriceDraft(60);
            var euro90 = TestingUtility.GetPriceDraft(90);


            var product1 =
                this.productProjectionsFixture.CreateCustomizedProduct(prices: new List<PriceDraft> {euro30});
            var product2 =
                this.productProjectionsFixture.CreateCustomizedProduct(prices: new List<PriceDraft> {euro60});
            var product3 =
                this.productProjectionsFixture.CreateCustomizedProduct(prices: new List<PriceDraft> {euro90});


            string[] productIds = new[]
            {
                product1.Id, product2.Id, product3.Id
            };

            //Act (filter products with specific price and return facet of 2 ranges for products with prices)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.Filter(p =>
                p.Variants.Any(v => v.Price.Value.CentAmount == euro30.Value.CentAmount.valueOf()));
            searchRequest.RangeFacet("ProductsBelow70",
                p => p.Variants.Any(v => v.Price.Value.CentAmount.Range(null, 70)));
            searchRequest.RangeFacet("ProductsFrom50To100",
                p => p.Variants.Any(v => v.Price.Value.CentAmount.Range(50, 100)));

            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();
                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == product1.Id);

                Assert.Equal(2, searchResults.Facets.Count);
                var productsBelow70 = searchResults.Facets["ProductsBelow70"] as RangeFacetResult;
                var productsFrom50To100 = searchResults.Facets["ProductsFrom50To100"] as RangeFacetResult;

                Assert.NotNull(productsBelow70);
                Assert.Single(productsBelow70.Ranges);
                Assert.Equal(2, productsBelow70.Ranges[0].Count);
                Assert.Equal(euro30.Value.CentAmount, productsBelow70.Ranges[0].Min);
                Assert.Equal(euro60.Value.CentAmount, productsBelow70.Ranges[0].Max);


                Assert.NotNull(productsFrom50To100);
                Assert.Single(productsFrom50To100.Ranges);
                Assert.Equal(2, productsFrom50To100.Ranges[0].Count);
                Assert.Equal(euro60.Value.CentAmount, productsFrom50To100.Ranges[0].Min);
                Assert.Equal(euro90.Value.CentAmount, productsFrom50To100.Ranges[0].Max);
            });
        }


        [Fact]
        public void TestFilteredFacetWithProductCategories()
        {
            //Arrange
            var allCategories = this.productProjectionsFixture.SetupCategories();
            Assert.True(allCategories.Count > 0);

            var categoryA = allCategories.FirstOrDefault(c => c.ExternalId == "A");
            var categoryA1 = allCategories.FirstOrDefault(c => c.ExternalId == "A1");
            var categoryB = allCategories.FirstOrDefault(c => c.ExternalId == "B");

            Assert.NotNull(categoryA);
            Assert.NotNull(categoryA1);
            Assert.NotNull(categoryB);

            var productA = this.productProjectionsFixture.CreateProduct(productCategory: categoryA);
            var productA1 = this.productProjectionsFixture.CreateProduct(productCategory: categoryA1);
            var productB = this.productProjectionsFixture.CreateProduct(productCategory: categoryB);

            var productIds = new[] {productA.Id, productA1.Id, productB.Id};

            //Act
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.FilteredFacet("ProductsInA",
                p => p.Categories.Any(c => c.Id.Subtree(categoryA.Id.valueOf())));
            searchRequest.FilteredFacet("ProductsInB",
                p => p.Categories.Any(c => c.Id.Subtree(categoryB.Id.valueOf())));

            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();
                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Equal(3, searchResults.Results.Count);
                Assert.Equal(2, searchResults.Facets.Count);

                var productsInAFacetResult = searchResults.Facets["ProductsInA"] as FilteredFacetResult;
                var productsInBFacetResult = searchResults.Facets["ProductsInB"] as FilteredFacetResult;

                Assert.NotNull(productsInAFacetResult);
                Assert.NotNull(productsInBFacetResult);
                Assert.Equal(2, productsInAFacetResult.Count);

                Assert.Equal(1, productsInBFacetResult.Count);
            });
        }

        [Fact]
        public void TestFilteredFacetByPriceCentAmount()
        {
            //Arrange
            var euro30 = TestingUtility.GetPriceDraft(30);
            var euro60 = TestingUtility.GetPriceDraft(60);
            var euro90 = TestingUtility.GetPriceDraft(90);


            var product1 =
                this.productProjectionsFixture.CreateCustomizedProduct(prices: new List<PriceDraft> {euro30});
            var product2 =
                this.productProjectionsFixture.CreateCustomizedProduct(prices: new List<PriceDraft> {euro60});
            var product3 =
                this.productProjectionsFixture.CreateCustomizedProduct(prices: new List<PriceDraft> {euro90});


            string[] productIds = new[]
            {
                product1.Id, product2.Id, product3.Id
            };

            //Act (filter products with specific price and return filtered facet for products with prices)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.Filter(p =>
                p.Variants.Any(v => v.Price.Value.CentAmount == euro30.Value.CentAmount.valueOf()));

            searchRequest.FilteredFacet("Products60",
                p => p.Variants.Any(v => v.Price.Value.CentAmount == euro60.Value.CentAmount.valueOf()),
                isCountingProducts: false);
            searchRequest.FilteredFacet("Products90",
                p => p.Variants.Any(v => v.Price.Value.CentAmount == euro90.Value.CentAmount.valueOf()),
                isCountingProducts: true);

            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();
                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == product1.Id);

                Assert.Equal(2, searchResults.Facets.Count);
                var products60 = searchResults.Facets["Products60"] as FilteredFacetResult;
                var products90 = searchResults.Facets["Products90"] as FilteredFacetResult;

                Assert.NotNull(products60);
                Assert.True(products60.Count == 1);
                Assert.True(products60.ProductCount == 0);

                Assert.NotNull(products90);
                Assert.True(products90.Count == 1);
                Assert.True(products90.ProductCount == 1);
            });
        }

        [Fact]
        public void TestFilteredFacetNumberAttribute()
        {
            //Arrange
            var value10 = 10.0;
            var value30 = 30.0;
            var value50 = 50.0;

            var product1 = this.productProjectionsFixture.CreateCustomizedProduct(
                attributes: new List<Attribute>
                    {new NumberAttribute {Name = "number-attribute-name", Value = value10}});
            var product2 = this.productProjectionsFixture.CreateCustomizedProduct(
                attributes: new List<Attribute>
                    {new NumberAttribute {Name = "number-attribute-name", Value = value30}});
            var product3 = this.productProjectionsFixture.CreateCustomizedProduct(
                attributes: new List<Attribute>
                    {new NumberAttribute {Name = "number-attribute-name", Value = value50}});

            var productIds = new[] {product1.Id, product2.Id, product3.Id};

            //Act (filter only products which have custom number attribute with value 50, this will influence facet counts)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.FilterQuery(p => p.Variants.Any(v =>
                v.Attributes.Any(a => a.Name == "number-attribute-name" && ((NumberAttribute) a).Value == value50.valueOf())));

            searchRequest.FilteredFacet("ProductsByValue30Or50", p => p.Variants.Any(v =>
                v.Attributes.Any(a => a.Name == "number-attribute-name" && ((NumberAttribute)a).Value.In(new[]{ value30, value50}))));


            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();

                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == product3.Id);

                Assert.Single(searchResults.Facets);


                var facetResult = searchResults.Facets["ProductsByValue30Or50"] as FilteredFacetResult;
                Assert.NotNull(facetResult);
                Assert.Equal(1, facetResult.Count);//because we're using filterQuery which will affect facet count
            });
        }

        #endregion


        #region Sort

        [Fact]
        public void SortByName()
        {
            var randInt = TestingUtility.RandomInt();
            //Arrange
            var product1 =
                this.productProjectionsFixture.CreateProductWithLocalizedName(new LocalizedString
                {
                    {"en", $"b{randInt}"}
                });
            var product2 =
                this.productProjectionsFixture.CreateProductWithLocalizedName(new LocalizedString
                {
                    {"en", $"a{randInt}"}
                });
            var product3 =
                this.productProjectionsFixture.CreateProductWithLocalizedName(new LocalizedString
                {
                    {"en", $"c{randInt}"}
                });


            string[] productIds = new[] { product1.Id, product2.Id, product3.Id };

            //Act
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.Sort(p => p.Name["en"], SortDirection.Descending);

            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();

                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Equal(3, searchResults.Results.Count);

                Assert.Equal(product3.Id, searchResults.Results[0].Id);
                Assert.Equal(product1.Id, searchResults.Results[1].Id);
                Assert.Equal(product2.Id, searchResults.Results[2].Id);
            });
        }

        [Fact]
        public void SortByProductAvailability()
        {
            //Arrange
            var productAvailableWithQuantity3 = this.productProjectionsFixture.CreateCustomizedProduct(
                sku: TestingUtility.RandomString(10),
                productVariantAvailability: new ProductVariantAvailability
                {
                    AvailableQuantity = 3
                });

            var productAvailableWithQuantity6 = this.productProjectionsFixture.CreateCustomizedProduct(
                sku: TestingUtility.RandomString(10),
                productVariantAvailability: new ProductVariantAvailability
                {
                    AvailableQuantity = 6
                });

            string[] productIds = new[]
            {
                productAvailableWithQuantity3.Id,
                productAvailableWithQuantity6.Id
            };

            //Act
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.Sort(p => p.Variants.Select(v => v.Availability.AvailableQuantity).FirstOrDefault(), SortDirection.Descending);

            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();

                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Equal(2, searchResults.Results.Count);
                Assert.Equal(productAvailableWithQuantity6.Id, searchResults.Results[0].Id);
            });
        }

        [Fact]
        public void SortByProductAvailabilityWithSupplyChannel()
        {
            //Arrange
            var channel = this.productProjectionsFixture.CreateNewChannel(ChannelRole.InventorySupply);
            var productAvailableWithQuantity3 = this.productProjectionsFixture.CreateCustomizedProduct(
                sku: TestingUtility.RandomString(10),
                productVariantAvailability: new ProductVariantAvailability
                {
                    AvailableQuantity = 3
                }, supplyChannel: channel);

            var productAvailableWithQuantity6 = this.productProjectionsFixture.CreateCustomizedProduct(
                sku: TestingUtility.RandomString(10),
                productVariantAvailability: new ProductVariantAvailability
                {
                    AvailableQuantity = 6
                }, supplyChannel: channel);

            string[] productIds = new[]
            {
                productAvailableWithQuantity3.Id,
                productAvailableWithQuantity6.Id
            };

            //Act
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.Sort(p =>
                p.Variants.Select(v => v.Availability.Channels[channel.Id.valueOf()].AvailableQuantity)
                    .FirstOrDefault(), SortDirection.Descending);

            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();

                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Equal(2, searchResults.Results.Count);
                Assert.Equal(productAvailableWithQuantity6.Id, searchResults.Results[0].Id);
            });
        }

        [Fact]
        public void SortByProductRestockableInDays()
        {
            //Arrange
            var productRestockableInDays3 = this.productProjectionsFixture.CreateCustomizedProduct(
                sku: TestingUtility.RandomString(10),
                productVariantAvailability: new ProductVariantAvailability
                {
                    AvailableQuantity = 5,
                    RestockableInDays = 3
                });

            var productRestockableInDays6 = this.productProjectionsFixture.CreateCustomizedProduct(
                sku: TestingUtility.RandomString(10),
                productVariantAvailability: new ProductVariantAvailability
                {
                    AvailableQuantity = 5,
                    RestockableInDays = 6
                });

            string[] productIds = new[]
            {
                productRestockableInDays3.Id,
                productRestockableInDays6.Id
            };

            //Act
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.Sort(p => p.Variants.Select(v => v.Availability.RestockableInDays).FirstOrDefault(), SortDirection.Descending);

            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();

                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Equal(2, searchResults.Results.Count);
                Assert.Equal(productRestockableInDays6.Id, searchResults.Results[0].Id);
            });
        }

        [Fact]
        public void SortByReviewAverageRating()
        {
            //Arrange
            var product1 =
                this.productProjectionsFixture.CreateCustomizedProduct(productKey: TestingUtility.RandomString(10));
            this.productProjectionsFixture.CreateProductReview(product1.Key, 1);
            this.productProjectionsFixture.CreateProductReview(product1.Key, 3);

            var product2 =
                this.productProjectionsFixture.CreateCustomizedProduct(productKey: TestingUtility.RandomString(10));
            this.productProjectionsFixture.CreateProductReview(product2.Key, 3);
            this.productProjectionsFixture.CreateProductReview(product2.Key, 5);

            var product3 =
                this.productProjectionsFixture.CreateCustomizedProduct(productKey: TestingUtility.RandomString(10));
            this.productProjectionsFixture.CreateProductReview(product3.Key, 1);
            this.productProjectionsFixture.CreateProductReview(product3.Key, 1);

            string[] productIds = new[] { product1.Id, product2.Id, product3.Id };
            //Act
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.Sort(p => p.ReviewRatingStatistics.AverageRating, SortDirection.Descending);
            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();

                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Equal(3, searchResults.Results.Count);

                Assert.Equal(product2.Id, searchResults.Results[0].Id);
                Assert.Equal(4, searchResults.Results[0].ReviewRatingStatistics.AverageRating);

                Assert.Equal(product1.Id, searchResults.Results[1].Id);
                Assert.Equal(2, searchResults.Results[1].ReviewRatingStatistics.AverageRating);

                Assert.Equal(product3.Id, searchResults.Results[2].Id);
                Assert.Equal(1, searchResults.Results[2].ReviewRatingStatistics.AverageRating);
            });
        }

        [Fact]
        public void SortBySku()
        {
            var randInt = TestingUtility.RandomInt();
            //Arrange
            var product1 =
                this.productProjectionsFixture.CreateCustomizedProduct(
                    sku: $"b{randInt}");

            var product2 =
                this.productProjectionsFixture.CreateCustomizedProduct(
                    sku: $"a{randInt}");

            var product3 =
                this.productProjectionsFixture.CreateCustomizedProduct(
                    sku: $"c{randInt}");

            string[] productIds = new[] { product1.Id, product2.Id, product3.Id };
            //Act
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.Sort(p => p.Variants.Select(v=>v.Sku).FirstOrDefault(), SortDirection.Ascending);
            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();

                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Equal(3, searchResults.Results.Count);

                Assert.Equal(product2.Id, searchResults.Results[0].Id);
                Assert.Equal(product1.Id, searchResults.Results[1].Id);
                Assert.Equal(product3.Id, searchResults.Results[2].Id);
            });
        }

        [Fact]
        public void SortByCustomMoneyAttributeValue()
        {
            //Arrange
            var money30 = new Money {CentAmount = 30, CurrencyCode = "EUR"};
            var money50 = new Money {CentAmount = 50, CurrencyCode = "EUR"};
            var money80 = new Money {CentAmount = 80, CurrencyCode = "EUR"};


            var product1 = this.productProjectionsFixture.CreateCustomizedProduct(
                attributes: new List<Attribute> {new MoneyAttribute {Name = "money-attribute-name", Value = money80}});
            var product2 = this.productProjectionsFixture.CreateCustomizedProduct(
                attributes: new List<Attribute> {new MoneyAttribute {Name = "money-attribute-name", Value = money50}});
            var product3 = this.productProjectionsFixture.CreateCustomizedProduct(
                attributes: new List<Attribute> {new MoneyAttribute {Name = "money-attribute-name", Value = money30}});


            var productIds = new[] {product1.Id, product2.Id, product3.Id};

            //Act
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.Sort(p => p.Variants.Select(v => v.Attributes.Where(a => a.Name == "money-attribute-name").Select(a => ((MoneyAttribute)a).Value.CentAmount.ToString()).FirstOrDefault()).FirstOrDefault(), SortDirection.Ascending);

            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();

                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Equal(3, searchResults.Results.Count);

                Assert.Equal(product3.Id, searchResults.Results[0].Id);
                Assert.Equal(product2.Id, searchResults.Results[1].Id);
                Assert.Equal(product1.Id, searchResults.Results[2].Id);
            });
        }

        [Fact]
        public void SortByEnumCustomAttributeLabel()
        {
            //Arrange
            var product1 = this.productProjectionsFixture.CreateCustomizedProduct(
                attributes: new List<Attribute>
                {
                    new EnumAttribute
                    {
                        Name = "enum-attribute-name", Value = new PlainEnumValue
                        {
                            Key = "enum-key-2",
                            Label = "enum-label-2"
                        }
                    }
                });
            var product2 = this.productProjectionsFixture.CreateCustomizedProduct(
                attributes: new List<Attribute>
                {
                    new EnumAttribute
                    {
                        Name = "enum-attribute-name", Value = new PlainEnumValue
                        {
                            Key = "enum-key-1",
                            Label = "enum-label-1"
                        }
                    }
                });

            var product3 = this.productProjectionsFixture.CreateCustomizedProduct(
                attributes: new List<Attribute>
                {
                    new EnumAttribute
                    {
                        Name = "enum-attribute-name", Value = new PlainEnumValue
                        {
                            Key = "enum-key-3",
                            Label = "enum-label-3"
                        }
                    }
                });

            string[] productIds = new[]
            {
                product1.Id,
                product2.Id,
                product3.Id
            };

            //Act
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.Sort(p => p.Variants.Select(v => v.Attributes.Where(a => a.Name == "enum-attribute-name").Select(a => ((EnumAttribute)a).Value.Label).FirstOrDefault()).FirstOrDefault(), SortDirection.Descending);

            //Assert
            this.productProjectionsFixture.AssertEventually(() =>
            {
                IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();
                var searchResults = commerceToolsClient.ExecuteAsync(searchRequest).Result;
                Assert.Equal(3, searchResults.Results.Count);

                Assert.Equal(product3.Id, searchResults.Results[0].Id);
                Assert.Equal(product1.Id, searchResults.Results[1].Id);
                Assert.Equal(product2.Id, searchResults.Results[2].Id);
            });
        }

        #endregion
    }
}
