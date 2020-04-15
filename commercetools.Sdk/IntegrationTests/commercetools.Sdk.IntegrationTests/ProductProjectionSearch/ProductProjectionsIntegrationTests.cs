using System;
using System.Linq;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.Domain.ProductProjections;
using commercetools.Sdk.Domain.Products.Attributes;
using commercetools.Sdk.Domain.Query;
using Xunit;
using static commercetools.Sdk.IntegrationTests.GenericFixture;

namespace commercetools.Sdk.IntegrationTests.ProductProjectionSearch
{
    [Collection("Integration Tests")]
    public class ProductProjectionsIntegrationTests : IClassFixture<ProductProjectionSearchFixture>
    {
        private readonly string NotReadyForSearchMessage = "Products still not indexed yet and not ready for search";
        private readonly IClient client;
        private readonly ProductProjectionSearchFixture fixture;

        public ProductProjectionsIntegrationTests(ProductProjectionSearchFixture fixture)
        {
            this.fixture = fixture;
            this.client = this.fixture.ServiceProviderFixture.GetService<IClient>();
        }

        [Fact]
        public void SearchByFullLocale()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var product = this.fixture.FindProduct(p =>
                p.Name?.Keys.FirstOrDefault(k => k.ToString() == "de") != null);

            Assert.NotNull(product);

            var productIds = new[] {product.Id};

            var searchParams = new ProductProjectionSearchParameters
            {
                Text = new TextSearch
                {
                    Term = product.Name["de"],
                    Language = "de"
                }
            };
            //Act
            var searchRequest = new SearchProductProjectionsCommand(searchParams);
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));

            //Assert
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Equal(product.Id, searchResults.Results[0].Id);
                Assert.Equal(product.Name["en"], searchResults.Results[0].Name["en"]);
            });
        }

        [Fact]
        public void SearchWithFuzzyLevel()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var product = this.fixture.FindProduct(p =>
                p.Name != null && !string.IsNullOrEmpty(p.Name["en"]));
            Assert.NotNull(product);
            var productIds = new[] {product.Id};

            //Adding additional fuzzy character
            var term = product.Name["en"] + "z";
            var searchParams = new ProductProjectionSearchParameters
            {
                Text = new TextSearch {Term = term, Language = "en"},
                Fuzzy = true,
                FuzzyLevel = 1
            };

            //Act
            var searchRequest = new SearchProductProjectionsCommand(searchParams);
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));

            //Assert
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Equal(product.Id, searchResults.Results[0].Id);
                Assert.Equal(product.Name["en"], searchResults.Results[0].Name["en"]);
            });
        }

        #region Filters

        [Fact]
        public void FilterBySpecificCategoriesWithReferenceExpansion()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var productInCategory =
                this.fixture.FindProduct(p => p.Categories.Count > 0);
            Assert.NotNull(productInCategory);
            var productIds = new[] {productInCategory.Id};
            var categoryId = productInCategory.Categories.FirstOrDefault()?.Id;

            //Act
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.FilterQuery(p => p.Categories.Any(c => c.Id == categoryId));
            searchRequest.Expand(p => p.Categories.ExpandAll());

            //Assert
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Equal(productInCategory.Id, searchResults.Results[0].Id);

                //check reference expansion
                Assert.Single(searchResults.Results[0].Categories);
                Assert.NotNull(searchResults.Results[0].Categories[0].Obj);
                Assert.Equal(categoryId, searchResults.Results[0].Categories[0].Obj.Id);
            });
        }

        [Fact]
        public void FilterByCategoriesExists()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var productWithoutCategories =
                this.fixture.FindProduct(p => p.Categories.Count == 0);
            var productWithCategories =
                this.fixture.FindProduct(p => p.Categories.Count > 0);
            Assert.NotNull(productWithoutCategories);
            Assert.NotNull(productWithCategories);
            var productIds = new[]
            {
                productWithoutCategories.Id,
                productWithCategories.Id
            };

            //Act
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.FilterQuery(p => p.Categories.Exists());

            //Assert
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Equal(productWithCategories.Id, searchResults.Results[0].Id);
            });
        }

        [Fact]
        public void FilterByCategoriesMissing()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var productWithoutCategories =
                this.fixture.FindProduct(p => p.Categories.Count == 0);
            var productWithCategories =
                this.fixture.FindProduct(p => p.Categories.Count > 0);
            Assert.NotNull(productWithoutCategories);
            Assert.NotNull(productWithCategories);
            var productIds = new[]
            {
                productWithoutCategories.Id,
                productWithCategories.Id
            };

            //Act
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.FilterQuery(p => p.Categories.Missing());

            //Assert
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Equal(productWithoutCategories.Id, searchResults.Results[0].Id);
            });
        }

        [Fact]
        public void FilterBySpecificCategorySubtree()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var categoryA = this.fixture.FindCategory(c => c.ExternalId == "A");
            var categoryA1 = this.fixture.FindCategory(c => c.ExternalId == "A1");
            var categoryB = this.fixture.FindCategory(c => c.ExternalId == "B");
            Assert.NotNull(categoryA);
            Assert.NotNull(categoryA1);
            Assert.NotNull(categoryB);

            var productA = this.fixture.FindProduct(p =>
                p.Categories.Any(c => c.Id == categoryA.Id));
            var productA1 = this.fixture.FindProduct(p =>
                p.Categories.Any(c => c.Id == categoryA1.Id));
            var productB = this.fixture.FindProduct(p =>
                p.Categories.Any(c => c.Id == categoryB.Id));

            Assert.NotNull(productA);
            Assert.NotNull(productA1);
            Assert.NotNull(productB);

            var productIds = new[] {productA.Id, productA1.Id, productB.Id};

            //Act (filter products that belong to categoryA or any of its descendant categories)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.FilterQuery(p => p.Categories.Any(c => c.Id.Subtree(categoryA.Id.valueOf())));

            //Assert
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Equal(2, searchResults.Results.Count);
                Assert.Contains(searchResults.Results, p => p.Id == productA.Id);
                Assert.Contains(searchResults.Results, p => p.Id == productA1.Id);
                Assert.DoesNotContain(searchResults.Results, p => p.Id == productB.Id);
            });
        }

        [Fact]
        public void FilterByMoreThanOneCategorySubtree()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var categoryA = this.fixture.FindCategory(c => c.ExternalId == "A");
            var categoryA1 = this.fixture.FindCategory(c => c.ExternalId == "A1");
            var categoryB = this.fixture.FindCategory(c => c.ExternalId == "B");
            var categoryC = this.fixture.FindCategory(c => c.ExternalId == "C");

            Assert.NotNull(categoryA);
            Assert.NotNull(categoryA1);
            Assert.NotNull(categoryB);
            Assert.NotNull(categoryC);

            var productA = this.fixture.FindProduct(p =>
                p.Categories.Any(c => c.Id == categoryA.Id));
            var productA1 = this.fixture.FindProduct(p =>
                p.Categories.Any(c => c.Id == categoryA1.Id));
            var productB = this.fixture.FindProduct(p =>
                p.Categories.Any(c => c.Id == categoryB.Id));
            var productC = this.fixture.FindProduct(p =>
                p.Categories.Any(c => c.Id == categoryC.Id));

            var productIds = new[] {productA.Id, productA1.Id, productB.Id, productC.Id};

            //Act (filter products that belong to categoryA or categoryC or any their descendant categories)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.FilterQuery(p =>
                p.Categories.Any(c => c.Id.Subtree(categoryA.Id.valueOf()) || c.Id.Subtree(categoryC.Id.valueOf())));

            //Assert
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
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
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var categoryA = this.fixture.FindCategory(c => c.ExternalId == "A");
            var categoryA1 = this.fixture.FindCategory(c => c.ExternalId == "A1");
            var categoryB = this.fixture.FindCategory(c => c.ExternalId == "B");
            var categoryB1 = this.fixture.FindCategory(c => c.ExternalId == "B1");

            Assert.NotNull(categoryA);
            Assert.NotNull(categoryA1);
            Assert.NotNull(categoryB);
            Assert.NotNull(categoryB1);

            var productA = this.fixture.FindProduct(p =>
                p.Categories.Any(c => c.Id == categoryA.Id));
            var productA1 = this.fixture.FindProduct(p =>
                p.Categories.Any(c => c.Id == categoryA1.Id));
            var productB = this.fixture.FindProduct(p =>
                p.Categories.Any(c => c.Id == categoryB.Id));
            var productB1 = this.fixture.FindProduct(p =>
                p.Categories.Any(c => c.Id == categoryB1.Id));

            var productIds = new[] {productA.Id, productA1.Id, productB.Id, productB1.Id};

            //Act (filter products that belong to categoryA or any of it's descendant categories and category B1)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p =>
                p.Categories.Any(c => c.Id.Subtree(categoryA.Id.valueOf()) || c.Id == categoryB1.Id.valueOf()));


            //Assert
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
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
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var product30 = this.fixture.FindProduct(p =>
                p.MasterVariant.Prices.Any(price =>
                    price.Value.CentAmount == TestingUtility.Euro30.Value.CentAmount));
            var product50 = this.fixture.FindProduct(p =>
                p.MasterVariant.Prices.Any(price =>
                    price.Value.CentAmount == TestingUtility.Euro50.Value.CentAmount));
            var product70 = this.fixture.FindProduct(p =>
                p.MasterVariant.Prices.Any(price =>
                    price.Value.CentAmount == TestingUtility.Euro70.Value.CentAmount));

            Assert.NotNull(product30);
            Assert.NotNull(product50);
            Assert.NotNull(product70);
            var productIds = new[] {product30.Id, product50.Id, product70.Id};


            //Act
            //(filter by specific price)
            var filterBySpecificPrice = new SearchProductProjectionsCommand();
            filterBySpecificPrice.SetStaged(true);
            filterBySpecificPrice.FilterQuery(p => p.Id.In(productIds));
            filterBySpecificPrice.FilterQuery(p =>
                p.Variants.Any(v =>
                    v.Price.Value.CentAmount == TestingUtility.Euro30.Value.CentAmount.valueOf()));

            //(filter by range price to)
            var filterByRangeToPrice = new SearchProductProjectionsCommand();
            filterByRangeToPrice.SetStaged(true);
            filterByRangeToPrice.FilterQuery(p => p.Id.In(productIds));
            filterByRangeToPrice.FilterQuery(p =>
                p.Variants.Any(v =>
                    v.Price.Value.CentAmount.Range(null,
                        TestingUtility.Euro50.Value.CentAmount.valueOf())));

            //(filter by range price from - to)
            var filterByRangeFromToPrice = new SearchProductProjectionsCommand();
            filterByRangeFromToPrice.SetStaged(true);
            filterByRangeFromToPrice.FilterQuery(p => p.Id.In(productIds));
            filterByRangeFromToPrice.FilterQuery(p => p.Variants.Any(v => v.Price.Value.CentAmount.Range(40, 80)));

            //Assert
            AssertEventually(() =>
            {
                var filterBySpecificPriceResult = client.ExecuteAsync(filterBySpecificPrice).Result;
                Assert.Single(filterBySpecificPriceResult.Results);
                Assert.Contains(filterBySpecificPriceResult.Results, p => p.Id == product30.Id);

                var filterByRangeToPriceResult = client.ExecuteAsync(filterByRangeToPrice).Result;
                Assert.Equal(2, filterByRangeToPriceResult.Results.Count);
                Assert.Contains(filterByRangeToPriceResult.Results, p => p.Id == product30.Id);
                Assert.Contains(filterByRangeToPriceResult.Results, p => p.Id == product50.Id);

                var filterByRangeFromToPriceResult = client.ExecuteAsync(filterByRangeFromToPrice).Result;
                Assert.Equal(2, filterByRangeFromToPriceResult.Results.Count);
                Assert.Contains(filterByRangeFromToPriceResult.Results, p => p.Id == product50.Id);
                Assert.Contains(filterByRangeFromToPriceResult.Results, p => p.Id == product70.Id);
            });
        }

        [Fact]
        public void FilterByPricesMissing()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var productWithPrices =
                this.fixture.FindProduct(p =>
                    p.MasterVariant.Prices.Any(price =>
                        price.Value.CentAmount == TestingUtility.Euro30.Value.CentAmount));
            var productWithoutPrices = this.fixture.FindProduct(p =>
                p.MasterVariant.Prices.Count == 0);

            Assert.NotNull(productWithPrices);
            Assert.NotNull(productWithoutPrices);

            var productIds = new[]
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
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == productWithoutPrices.Id);
            });
        }

        [Fact]
        public void FilterByPricesExists()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var productWithPrices =
                this.fixture.FindProduct(p =>
                    p.MasterVariant.Prices.Any(price =>
                        price.Value.CentAmount == TestingUtility.Euro30.Value.CentAmount));
            var productWithoutPrices = this.fixture.FindProduct(p =>
                p.MasterVariant.Prices.Count == 0);

            Assert.NotNull(productWithPrices);
            Assert.NotNull(productWithoutPrices);

            var productIds = new[]
            {
                productWithPrices.Id,
                productWithoutPrices.Id
            };

            //Act (filter only products with price set)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.FilterQuery(p => p.Variants.Any(v => v.Prices.Exists()));

            //Assert
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == productWithPrices.Id);
            });
        }


        [Fact]
        public void FilterByScopedPriceValue()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var product1 = this.fixture.FindProduct(p =>
                p.MasterVariant.Prices.Any(price =>
                    price.Value.CentAmount == TestingUtility.EuroScoped40.Value.CentAmount));
            var product2 = this.fixture.FindProduct(p =>
                p.MasterVariant.Prices.Any(price =>
                    price.Value.CentAmount == TestingUtility.EuroScoped60.Value.CentAmount));

            Assert.NotNull(product1);
            Assert.NotNull(product2);
            var productIds = new[]
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
            searchRequest.FilterQuery(p => p.Variants.Any(v =>
                v.ScopedPrice.Value.CentAmount.Range(null, TestingUtility.EuroScoped40.Value.CentAmount.valueOf())));

            //Assert
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Equal(product1.Id, searchResults.Results[0].Id);
            });
        }

        [Fact]
        public void FilterByScopedPriceCurrentValue()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange (discount with 5 cent)
            var productWithDiscount = this.fixture.FindProduct(p =>
                p.MasterVariant.Prices.Any(price =>
                    price.Value.CentAmount == TestingUtility.EuroScoped100.Value.CentAmount));

            Assert.NotNull(productWithDiscount);
            var priceAfterDiscount = TestingUtility.EuroScoped100.Value.CentAmount - TestingUtility.DiscountOf5Euro;

            //Act
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.SetPriceCountry("DE");
            searchRequest.SetPriceCurrency("EUR");
            searchRequest.FilterQuery(p => p.Id == productWithDiscount.Id.valueOf());
            searchRequest.FilterQuery(p =>
                p.Variants.Any(v => v.ScopedPrice.CurrentValue.CentAmount.Range(null, priceAfterDiscount)));

            //Assert
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Equal(productWithDiscount.Id, searchResults.Results[0].Id);

                var masterVariant = searchResults.Results[0].MasterVariant;
                Assert.True(masterVariant.ScopedPriceDiscounted);
                Assert.NotNull(masterVariant.ScopedPrice);
                var scopedPrice = masterVariant.ScopedPrice;
                Assert.NotNull(scopedPrice.Discounted);
                Assert.Equal(TestingUtility.EuroScoped100.Value.CentAmount, scopedPrice.Value.CentAmount);
                Assert.Equal(priceAfterDiscount, scopedPrice.Discounted.Value.CentAmount);

                Assert.NotNull(masterVariant.Price);
                Assert.Equal(TestingUtility.EuroScoped100.Value.CentAmount, masterVariant.Price.Value.CentAmount);
                Assert.Equal(priceAfterDiscount, masterVariant.Price.Discounted.Value.CentAmount);
            });
        }

        [Fact]
        public void FilterByScopedPriceDiscountedValue()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange (discount with 5 cent)
            var productWithDiscount = this.fixture.FindProduct(p =>
                p.MasterVariant.Prices.Any(price =>
                    price.Value.CentAmount == TestingUtility.EuroScoped100.Value.CentAmount));

            Assert.NotNull(productWithDiscount);
            var priceAfterDiscount = TestingUtility.EuroScoped100.Value.CentAmount - TestingUtility.DiscountOf5Euro;

            //Act
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.SetPriceCountry("DE");
            searchRequest.SetPriceCurrency("EUR");
            searchRequest.FilterQuery(p => p.Id == productWithDiscount.Id.valueOf());
            searchRequest.FilterQuery(p =>
                p.Variants.Any(v => v.ScopedPrice.Discounted.Value.CentAmount.Range(null, priceAfterDiscount)));

            //Assert
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Equal(productWithDiscount.Id, searchResults.Results[0].Id);

                var masterVariant = searchResults.Results[0].MasterVariant;
                Assert.True(masterVariant.ScopedPriceDiscounted);
                Assert.NotNull(masterVariant.ScopedPrice);
                var scopedPrice = masterVariant.ScopedPrice;
                Assert.NotNull(scopedPrice.Discounted);
                Assert.Equal(TestingUtility.EuroScoped100.Value.CentAmount, scopedPrice.Value.CentAmount);
                Assert.Equal(priceAfterDiscount, scopedPrice.Discounted.Value.CentAmount);

                Assert.NotNull(masterVariant.Price);
                Assert.Equal(TestingUtility.EuroScoped100.Value.CentAmount, masterVariant.Price.Value.CentAmount);
                Assert.Equal(priceAfterDiscount, masterVariant.Price.Discounted.Value.CentAmount);
            });
        }

        [Fact]
        public void FilterByScopedPriceDiscounted()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange (discount with 5 cent)
            var productWithDiscount = this.fixture.FindProduct(p =>
                p.MasterVariant.Prices.Any(price =>
                    price.Value.CentAmount == TestingUtility.EuroScoped100.Value.CentAmount));

            Assert.NotNull(productWithDiscount);
            var priceAfterDiscount = TestingUtility.EuroScoped100.Value.CentAmount - TestingUtility.DiscountOf5Euro;

            //Act
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.SetPriceCountry("DE");
            searchRequest.SetPriceCurrency("EUR");
            searchRequest.FilterQuery(p => p.Id == productWithDiscount.Id.valueOf());
            searchRequest.FilterQuery(p => p.Variants.Any(v => v.ScopedPriceDiscounted == true));

            //Assert
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Equal(productWithDiscount.Id, searchResults.Results[0].Id);

                var masterVariant = searchResults.Results[0].MasterVariant;
                Assert.True(masterVariant.ScopedPriceDiscounted);
                Assert.NotNull(masterVariant.ScopedPrice);
                var scopedPrice = masterVariant.ScopedPrice;
                Assert.NotNull(scopedPrice.Discounted);
                Assert.Equal(TestingUtility.EuroScoped100.Value.CentAmount, scopedPrice.Value.CentAmount);
                Assert.Equal(priceAfterDiscount, scopedPrice.Discounted.Value.CentAmount);

                Assert.NotNull(masterVariant.Price);
                Assert.Equal(TestingUtility.EuroScoped100.Value.CentAmount, masterVariant.Price.Value.CentAmount);
                Assert.Equal(priceAfterDiscount, masterVariant.Price.Discounted.Value.CentAmount);
            });
        }


        [Fact]
        public void FilterBySpecificSku()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var productWithSku =
                this.fixture.FindProduct(p =>
                    !string.IsNullOrEmpty(p.MasterVariant.Sku));
            Assert.NotNull(productWithSku);
            var productIds = new[] {productWithSku.Id};

            //Act (filter by specific sku)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.FilterQuery(p =>
                p.Variants.Any(v => v.Sku == productWithSku.MasterVariant.Sku.valueOf()));

            //Assert
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == productWithSku.Id);
            });
        }

        [Fact]
        public void FilterBySkuMissing()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var productWithSku = this.fixture.FindProduct(p =>
                !string.IsNullOrEmpty(p.MasterVariant.Sku));
            var productWithoutSku = this.fixture.FindProduct(p =>
                string.IsNullOrEmpty(p.MasterVariant.Sku));

            Assert.NotNull(productWithSku);
            Assert.NotNull(productWithoutSku);

            var productIds = new[]
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
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == productWithoutSku.Id);
            });
        }

        [Fact]
        public void FilterBySkuExists()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var productWithSku = this.fixture.FindProduct(p =>
                !string.IsNullOrEmpty(p.MasterVariant.Sku));
            var productWithoutSku = this.fixture.FindProduct(p =>
                string.IsNullOrEmpty(p.MasterVariant.Sku));

            Assert.NotNull(productWithSku);
            Assert.NotNull(productWithoutSku);

            var productIds = new[]
            {
                productWithSku.Id,
                productWithoutSku.Id
            };

            //Act (filter only products with sku set)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.FilterQuery(p => p.Variants.Any(v => v.Sku.Exists()));

            //Assert
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == productWithSku.Id);
            });
        }


        [Fact]
        public void FilterBySpecificProductKey()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var productWithKey =
                this.fixture.FindProduct(p =>
                    !string.IsNullOrEmpty(p.Key));
            Assert.NotNull(productWithKey);
            var productIds = new[] {productWithKey.Id};

            //Act (filter by specific productKey)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.FilterQuery(p => p.Key == productWithKey.Key.valueOf());

            //Assert
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == productWithKey.Id);
            });
        }

        [Fact]
        public void FilterByProductKeyMissing()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var productWithKey = this.fixture.FindProduct(p =>
                !string.IsNullOrEmpty(p.Key));
            var productWithoutKey = this.fixture.FindProduct(p =>
                string.IsNullOrEmpty(p.Key));

            Assert.NotNull(productWithKey);
            Assert.NotNull(productWithoutKey);

            var productIds = new[]
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
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == productWithoutKey.Id);
            });
        }

        [Fact]
        public void FilterByProductKeyExists()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var productWithKey = this.fixture.FindProduct(p =>
                !string.IsNullOrEmpty(p.Key));
            var productWithoutKey = this.fixture.FindProduct(p =>
                string.IsNullOrEmpty(p.Key));

            Assert.NotNull(productWithKey);
            Assert.NotNull(productWithoutKey);

            var productIds = new[]
            {
                productWithKey.Id,
                productWithoutKey.Id
            };

            //Act (filter only products with key set)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.FilterQuery(p => p.Key.Exists());

            //Assert
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == productWithKey.Id);
            });
        }


        [Fact]
        public void FilterByProductVariantKey()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var productWithKey =
                this.fixture.FindProduct(p =>
                    !string.IsNullOrEmpty(p.MasterVariant.Key));
            Assert.NotNull(productWithKey);
            var productIds = new[] {productWithKey.Id};

            //Act (filter by specific variant key)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.FilterQuery(p =>
                p.Variants.Any(v => v.Key == productWithKey.MasterVariant.Key.valueOf()));

            //Assert
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == productWithKey.Id);
            });
        }

        [Fact]
        public void FilterByProductVariantKeyMissing()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var productWithKey = this.fixture.FindProduct(p =>
                !string.IsNullOrEmpty(p.MasterVariant.Key));
            var productWithoutKey = this.fixture.FindProduct(p =>
                string.IsNullOrEmpty(p.MasterVariant.Key));

            Assert.NotNull(productWithKey);
            Assert.NotNull(productWithoutKey);
            var productIds = new[]
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
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == productWithoutKey.Id);
            });
        }

        [Fact]
        public void FilterByProductVariantKeyExists()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var productWithKey = this.fixture.FindProduct(p =>
                !string.IsNullOrEmpty(p.MasterVariant.Key));
            var productWithoutKey = this.fixture.FindProduct(p =>
                string.IsNullOrEmpty(p.MasterVariant.Key));

            Assert.NotNull(productWithKey);
            Assert.NotNull(productWithoutKey);
            var productIds = new[]
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
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == productWithKey.Id);
            });
        }


        [Fact]
        public void FilterByProductTypeWithReferenceExpansion()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var productWithProductType = this.fixture.FindProduct(p => p.ProductType != null);
            Assert.NotNull(productWithProductType);
            var productIds = new[] {productWithProductType.Id};

            //Act (filter only the product which matches the specified ProductType)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.FilterQuery(p => p.ProductType.Id == productWithProductType.ProductType.Id.valueOf());
            searchRequest.Expand(p => p.ProductType);

            var searchRequestUsingFilter = new SearchProductProjectionsCommand();
            searchRequestUsingFilter.SetStaged(true);
            searchRequestUsingFilter.Filter(p => p.Id.In(productIds));
            searchRequestUsingFilter.Filter(p => p.ProductType.Id == productWithProductType.ProductType.Id.valueOf());

            //Assert
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == productWithProductType.Id);
                Assert.NotNull(searchResults.Results[0].ProductType.Obj);

                var searchResultsUsingFilter = client.ExecuteAsync(searchRequestUsingFilter).Result;
                Assert.Single(searchResultsUsingFilter.Results);
                Assert.Contains(searchResultsUsingFilter.Results, p => p.Id == productWithProductType.Id);
            });
        }

        [Fact]
        public void FilterBySpecificTaxCategory()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var productWithTaxCategory = this.fixture.FindProduct(p => p.TaxCategory != null);
            Assert.NotNull(productWithTaxCategory);
            var productIds = new[] {productWithTaxCategory.Id};

            //Act (filter only the product which matches the specified TaxCategory)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.FilterQuery(p => p.TaxCategory.Id == productWithTaxCategory.TaxCategory.Id.valueOf());

            var searchRequestUsingFilter = new SearchProductProjectionsCommand();
            searchRequestUsingFilter.SetStaged(true);
            searchRequestUsingFilter.Filter(p => p.Id.In(productIds));
            searchRequestUsingFilter.Filter(p => p.TaxCategory.Id == productWithTaxCategory.TaxCategory.Id.valueOf());

            //Assert
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == productWithTaxCategory.Id);

                var searchResultsUsingFilter = client.ExecuteAsync(searchRequestUsingFilter).Result;
                Assert.Single(searchResultsUsingFilter.Results);
                Assert.Contains(searchResultsUsingFilter.Results, p => p.Id == productWithTaxCategory.Id);
            });
        }

        [Fact]
        public void FilterByTaxCategoryMissing()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var productWithTaxCategory =
                this.fixture.FindProduct(p => p.TaxCategory != null);
            var productWithoutTaxCategory = this.fixture.FindProduct(p => p.TaxCategory == null);
            Assert.NotNull(productWithTaxCategory);
            Assert.NotNull(productWithoutTaxCategory);
            var productIds = new[] {productWithTaxCategory.Id, productWithoutTaxCategory.Id};

            //Act (filter only the product which have TaxCategory not set)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.TaxCategory.Missing());
            searchRequest.FilterQuery(p => p.Id.In(productIds));

            var searchRequestUsingFilter = new SearchProductProjectionsCommand();
            searchRequestUsingFilter.SetStaged(true);
            searchRequestUsingFilter.Filter(p => p.TaxCategory.Missing());
            searchRequestUsingFilter.Filter(p => p.Id.In(productIds));

            //Assert
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == productWithoutTaxCategory.Id);

                var searchResultsUsingFilter = client.ExecuteAsync(searchRequestUsingFilter).Result;
                Assert.Single(searchResultsUsingFilter.Results);
                Assert.Contains(searchResultsUsingFilter.Results, p => p.Id == productWithoutTaxCategory.Id);
            });
        }

        [Fact]
        public void FilterByTaxCategoryExists()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var productWithTaxCategory =
                this.fixture.FindProduct(p => p.TaxCategory != null);
            var productWithoutTaxCategory = this.fixture.FindProduct(p => p.TaxCategory == null);
            Assert.NotNull(productWithTaxCategory);
            Assert.NotNull(productWithoutTaxCategory);
            var productIds = new[] {productWithTaxCategory.Id, productWithoutTaxCategory.Id};

            //Act (filter only the product which have TaxCategory not set)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.TaxCategory.Exists());
            searchRequest.FilterQuery(p => p.Id.In(productIds));

            var searchRequestUsingFilter = new SearchProductProjectionsCommand();
            searchRequestUsingFilter.SetStaged(true);
            searchRequestUsingFilter.Filter(p => p.TaxCategory.Exists());
            searchRequestUsingFilter.Filter(p => p.Id.In(productIds));

            //Assert
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == productWithTaxCategory.Id);

                var searchResultsUsingFilter = client.ExecuteAsync(searchRequestUsingFilter).Result;
                Assert.Single(searchResultsUsingFilter.Results);
                Assert.Contains(searchResultsUsingFilter.Results, p => p.Id == productWithTaxCategory.Id);
            });
        }


        [Fact]
        public void FilterBySpecificState()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var productWithState = this.fixture.FindProduct(p => p.State != null);
            Assert.NotNull(productWithState);
            var productIds = new[] {productWithState.Id};

            //Act (filter only the product which matches the specified state)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.FilterQuery(p => p.State.Id == productWithState.State.Id.valueOf());

            var searchRequestUsingFilter = new SearchProductProjectionsCommand();
            searchRequestUsingFilter.SetStaged(true);
            searchRequestUsingFilter.Filter(p => p.Id.In(productIds));
            searchRequestUsingFilter.Filter(p => p.State.Id == productWithState.State.Id.valueOf());

            //Assert

            var searchResults = client.ExecuteAsync(searchRequest).Result;
            Assert.Single(searchResults.Results);
            Assert.Contains(searchResults.Results, p => p.Id == productWithState.Id);

            var searchResultsUsingFilter = client.ExecuteAsync(searchRequestUsingFilter).Result;
            Assert.Single(searchResultsUsingFilter.Results);
            Assert.Contains(searchResultsUsingFilter.Results, p => p.Id == productWithState.Id);
        }

        [Fact]
        public void FilterByStateMissing()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var productWithState = this.fixture.FindProduct(p => p.State != null);
            var productWithoutState = this.fixture.FindProduct(p => p.State == null);
            Assert.NotNull(productWithState);
            Assert.NotNull(productWithoutState);
            var productIds = new[] {productWithState.Id, productWithoutState.Id};

            //Act (filter only the product which matches the specified state)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.FilterQuery(p => p.State.Missing());

            var searchRequestUsingFilter = new SearchProductProjectionsCommand();
            searchRequestUsingFilter.SetStaged(true);
            searchRequestUsingFilter.Filter(p => p.Id.In(productIds));
            searchRequestUsingFilter.Filter(p => p.State.Missing());

            //Assert
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == productWithoutState.Id);

                var searchResultsUsingFilter = client.ExecuteAsync(searchRequestUsingFilter).Result;
                Assert.Single(searchResultsUsingFilter.Results);
                Assert.Contains(searchResultsUsingFilter.Results, p => p.Id == productWithoutState.Id);
            });
        }

        [Fact]
        public void FilterByStateExists()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var productWithState = this.fixture.FindProduct(p => p.State != null);
            var productWithoutState = this.fixture.FindProduct(p => p.State == null);
            Assert.NotNull(productWithState);
            Assert.NotNull(productWithoutState);
            var productIds = new[] {productWithState.Id, productWithoutState.Id};

            //Act (filter only the product which matches the specified state)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.State.Exists());
            searchRequest.FilterQuery(p => p.Id.In(productIds));

            var searchRequestUsingFilter = new SearchProductProjectionsCommand();
            searchRequestUsingFilter.SetStaged(true);
            searchRequestUsingFilter.Filter(p => p.State.Exists());
            searchRequestUsingFilter.Filter(p => p.Id.In(productIds));

            //Assert
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == productWithState.Id);

                var searchResultsUsingFilter = client.ExecuteAsync(searchRequestUsingFilter).Result;
                Assert.Single(searchResultsUsingFilter.Results);
                Assert.Contains(searchResultsUsingFilter.Results, p => p.Id == productWithState.Id);
            });
        }

        [Fact]
        public void FilterByReviewRatingStatistics()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var product = this.fixture.FindProduct(p => p.ReviewRatingStatistics != null);
            Assert.NotNull(product);
            var productIds = new[] {product.Id};
            var reviewRatingStatistics = product.ReviewRatingStatistics;
            //Act
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.FilterQuery(p =>
                p.ReviewRatingStatistics.AverageRating == reviewRatingStatistics.AverageRating.valueOf());
            searchRequest.FilterQuery(p =>
                p.ReviewRatingStatistics.LowestRating == reviewRatingStatistics.LowestRating.valueOf());
            searchRequest.FilterQuery(p =>
                p.ReviewRatingStatistics.HighestRating == reviewRatingStatistics.HighestRating.valueOf());
            searchRequest.FilterQuery(p => p.ReviewRatingStatistics.Count == reviewRatingStatistics.Count.valueOf());

            //Assert
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == product.Id);
            });
        }

        [Fact]
        public void FilterByCustomAttributeValue()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var product30 = this.fixture.FindProduct(p =>
                p.Key == fixture.KeyProductWithPrice30);
            var product50 = this.fixture.FindProduct(p =>
                p.Key == fixture.KeyProductWithPrice50);
            var product70 = this.fixture.FindProduct(p =>
                p.Key == fixture.KeyProductWithPrice70);

            Assert.NotNull(product30);
            Assert.NotNull(product50);
            Assert.NotNull(product70);

            var productIds = new[] {product30.Id, product50.Id, product70.Id};

            //Act (filter only the product which have custom attribute value)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.FilterQuery(p => p.Variants.Any(v =>
                v.Attributes.Any(a => a.Name == "number-attribute-name" && ((NumberAttribute) a).Value == 30)));

            var searchRequestByRange = new SearchProductProjectionsCommand();
            searchRequestByRange.SetStaged(true);
            searchRequestByRange.FilterQuery(p => p.Id.In(productIds));
            searchRequestByRange.FilterQuery(p => p.Variants.Any(v =>
                v.Attributes.Any(a => a.Name == "number-attribute-name" && ((NumberAttribute) a).Value.Range(40, 70))));

            var searchRequestByRangeTo = new SearchProductProjectionsCommand();
            searchRequestByRangeTo.SetStaged(true);
            searchRequestByRangeTo.FilterQuery(p => p.Id.In(productIds));
            searchRequestByRangeTo.FilterQuery(p => p.Variants.Any(v =>
                v.Attributes.Any(a =>
                    a.Name == "number-attribute-name" && ((NumberAttribute) a).Value.Range(null, 50))));


            //Assert
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == product30.Id);

                var searchResultsByRange = client.ExecuteAsync(searchRequestByRange).Result;
                Assert.Equal(2, searchResultsByRange.Results.Count);
                Assert.Contains(searchResultsByRange.Results, p => p.Id == product50.Id);
                Assert.Contains(searchResultsByRange.Results, p => p.Id == product70.Id);

                var searchResultsByRangeTo = client.ExecuteAsync(searchRequestByRangeTo).Result;
                Assert.Equal(2, searchResultsByRangeTo.Results.Count);
                Assert.Contains(searchResultsByRangeTo.Results, p => p.Id == product30.Id);
                Assert.Contains(searchResultsByRangeTo.Results, p => p.Id == product50.Id);
            });
        }


        [Fact]
        public void FilterByCustomAttributeMissing()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var productWithCustomAttribute = this.fixture.FindProduct(p =>
                p.MasterVariant.Attributes.Any(a => a.Name == "number-attribute-name"));
            var productWithoutCustomAttribute =
                this.fixture.FindProduct(p =>
                    p.MasterVariant.Attributes.Count == 0);

            Assert.NotNull(productWithCustomAttribute);
            Assert.NotNull(productWithoutCustomAttribute);

            var productIds = new[]
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
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == productWithoutCustomAttribute.Id);
            });
        }

        [Fact]
        public void FilterByCustomAttributeExists()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var productWithCustomAttribute = this.fixture.FindProduct(p =>
                p.MasterVariant.Attributes.Any(a => a.Name == "number-attribute-name"));
            var productWithoutCustomAttribute =
                this.fixture.FindProduct(p =>
                    p.MasterVariant.Attributes.Count == 0);

            Assert.NotNull(productWithCustomAttribute);
            Assert.NotNull(productWithoutCustomAttribute);

            var productIds = new[]
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
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == productWithCustomAttribute.Id);
            });
        }

        [Fact]
        public void FilterByEnumCustomAttribute()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var productWithEnum1 = this.fixture.FindProduct(p =>
                p.Key == fixture.KeyProductWithPrice30);
            var productWithEnum2 = this.fixture.FindProduct(p =>
                p.Key == fixture.KeyProductWithPrice50);
            Assert.NotNull(productWithEnum1);
            Assert.NotNull(productWithEnum2);
            var productIds = new[]
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
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == productWithEnum1.Id);
            });
        }


        [Fact]
        public void FilterByMoneyCustomAttributeValue()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var product30 = this.fixture.FindProduct(p =>
                p.Key == fixture.KeyProductWithPrice30);
            var product50 = this.fixture.FindProduct(p =>
                p.Key == fixture.KeyProductWithPrice50);
            var product70 = this.fixture.FindProduct(p =>
                p.Key == fixture.KeyProductWithPrice70);

            Assert.NotNull(product30);
            Assert.NotNull(product50);
            Assert.NotNull(product70);

            var productIds = new[] {product30.Id, product50.Id, product70.Id};

            //Act (filter only the product which have money custom attribute with specific value)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.FilterQuery(p => p.Variants.Any(v => v.Attributes.Any(a =>
                a.Name == "money-attribute-name" &&
                ((MoneyAttribute) a).Value.CentAmount == TestingUtility.Money30.CentAmount.valueOf())));

            var searchByCurrencyRequest = new SearchProductProjectionsCommand();
            searchByCurrencyRequest.SetStaged(true);
            searchByCurrencyRequest.FilterQuery(p => p.Id.In(productIds));
            searchByCurrencyRequest.FilterQuery(p => p.Variants.Any(v => v.Attributes.Any(a =>
                a.Name == "money-attribute-name" &&
                ((MoneyAttribute) a).Value.CurrencyCode == TestingUtility.Money70.CurrencyCode.valueOf()))); //USD

            var searchByRangeRequest = new SearchProductProjectionsCommand();
            searchByRangeRequest.SetStaged(true);
            searchByRangeRequest.FilterQuery(p => p.Id.In(productIds));
            searchByRangeRequest.FilterQuery(p => p.Variants.Any(v => v.Attributes.Any(a =>
                a.Name == "money-attribute-name" && ((MoneyAttribute) a).Value.CentAmount.Range(10, 40))));


            //Assert
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == product30.Id);

                var searchByCurrencyResults = client.ExecuteAsync(searchByCurrencyRequest).Result;
                Assert.Single(searchByCurrencyResults.Results);
                Assert.Contains(searchByCurrencyResults.Results, p => p.Id == product70.Id);

                var searchByRangeResults = client.ExecuteAsync(searchByRangeRequest).Result;
                Assert.Single(searchByRangeResults.Results);
                Assert.Contains(searchByRangeResults.Results, p => p.Id == product30.Id);
            });
        }


        [Fact]
        public void FilterByReferenceCustomAttribute()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var productOfType1 = this.fixture.FindProduct(p =>
                p.Key == fixture.KeyProductWithPrice30);

            Assert.NotNull(productOfType1);
            var productIds = new[] {productOfType1.Id};
            var productType1 = this.fixture.AvailableProductType;

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
            AssertEventually(() =>
            {
                var searchByReferenceTypeIdResults =
                    client.ExecuteAsync(searchRequestByReferenceTypeId).Result;
                Assert.Single(searchByReferenceTypeIdResults.Results);
                Assert.Contains(searchByReferenceTypeIdResults.Results, p => p.Id == productOfType1.Id);

                var searchByProductTypeIdResults = client.ExecuteAsync(searchRequestByTypeId).Result;
                Assert.Single(searchByProductTypeIdResults.Results);
                Assert.Contains(searchByProductTypeIdResults.Results, p => p.Id == productOfType1.Id);
            });
        }


        [Fact]
        public void FilterByProductAvailability()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var productNotAvailableInStock = this.fixture.FindProduct(p =>
                p.Key == this.fixture.KeyProductWithPrice30WithoutChannel.valueOf());

            var productAvailableWithQuantity3 = this.fixture.FindProduct(p =>
                p.Key == this.fixture.KeyProductWithPrice50WithoutChannel.valueOf());

            var productAvailableWithQuantity6 = this.fixture.FindProduct(p =>
                p.Key == this.fixture.KeyProductWithPrice70WithoutChannel.valueOf());

            Assert.NotNull(productNotAvailableInStock);
            Assert.NotNull(productAvailableWithQuantity3);
            Assert.NotNull(productAvailableWithQuantity6);

            var productIds = new[]
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
            AssertEventually(() =>
            {
                var searchProductsOnStockResult = client.ExecuteAsync(searchProductsOnStock).Result;
                Assert.Equal(2, searchProductsOnStockResult.Results.Count);
                Assert.DoesNotContain(searchProductsOnStockResult.Results, p => p.Id == productNotAvailableInStock.Id);
                Assert.Contains(searchProductsOnStockResult.Results, p => p.Id == productAvailableWithQuantity3.Id);
                Assert.Contains(searchProductsOnStockResult.Results, p => p.Id == productAvailableWithQuantity6.Id);


                var searchProductsByAvailabilityResult =
                    client.ExecuteAsync(searchProductsByAvailability).Result;
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
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var productAvailableInChannel = this.fixture.FindProduct(p =>
                p.Key == this.fixture.KeyProductWithPrice50.valueOf());

            var productNotAvailableInChannel = this.fixture.FindProduct(p =>
                p.Key == this.fixture.KeyProductWithPrice30.valueOf());

            Assert.NotNull(productAvailableInChannel);
            Assert.NotNull(productNotAvailableInChannel);

            var channelId = this.fixture.AvailableChannels.FirstOrDefault()?.Id;

            var productIds = new[]
            {
                productAvailableInChannel.Id,
                productNotAvailableInChannel.Id
            };

            //Act
            var searchProductsOnStock = new SearchProductProjectionsCommand();
            searchProductsOnStock.SetStaged(true);
            searchProductsOnStock.FilterQuery(p => p.Id.In(productIds));
            searchProductsOnStock.FilterQuery(p =>
                p.Variants.Any(v => v.Availability.Channels[channelId].IsOnStock == true));

            var searchProductsNotAvailableOnStock = new SearchProductProjectionsCommand();
            searchProductsNotAvailableOnStock.SetStaged(true);
            searchProductsNotAvailableOnStock.FilterQuery(p => p.Id.In(productIds));
            searchProductsNotAvailableOnStock.FilterQuery(p =>
                p.Variants.Any(v => v.Availability.Channels[channelId].IsOnStock == false));

            //Assert
            AssertEventually(() =>
            {
                var searchProductsOnStockResult = client.ExecuteAsync(searchProductsOnStock).Result;
                Assert.Single(searchProductsOnStockResult.Results);
                Assert.Equal(productAvailableInChannel.Id, searchProductsOnStockResult.Results[0].Id);

                var searchProductsNotAvailableOnStockResult =
                    client.ExecuteAsync(searchProductsNotAvailableOnStock).Result;
                Assert.Single(searchProductsNotAvailableOnStockResult.Results);
                Assert.Equal(productNotAvailableInChannel.Id, searchProductsNotAvailableOnStockResult.Results[0].Id);
            });
        }

        [Fact]
        public void FilterProductsByAvailabilityRangeWithChannel()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var productAvailableWithQuantity3 = this.fixture.FindProduct(p =>
                p.Key == this.fixture.KeyProductWithPrice50.valueOf());

            var productAvailableWithQuantity6 = this.fixture.FindProduct(p =>
                p.Key == this.fixture.KeyProductWithPrice70.valueOf());

            Assert.NotNull(productAvailableWithQuantity3);
            Assert.NotNull(productAvailableWithQuantity6);

            var productIds = new[]
            {
                productAvailableWithQuantity3.Id,
                productAvailableWithQuantity6.Id
            };
            var channelId = this.fixture.AvailableChannels.FirstOrDefault()?.Id;

            //Act
            var searchProductsByRange = new SearchProductProjectionsCommand();
            searchProductsByRange.SetStaged(true);
            searchProductsByRange.FilterQuery(p => p.Id.In(productIds));
            searchProductsByRange.FilterQuery(p =>
                p.Variants.Any(v => v.Availability.Channels[channelId].AvailableQuantity.Range(4, 6)));


            //Assert
            AssertEventually(() =>
            {
                var searchProductsByRangeResult = client.ExecuteAsync(searchProductsByRange).Result;
                Assert.Single(searchProductsByRangeResult.Results);
                Assert.Equal(productAvailableWithQuantity6.Id, searchProductsByRangeResult.Results[0].Id);
            });
        }

        [Fact]
        public void FilterProductsOnStockWithSpecificChannels()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var channel1 = this.fixture.AvailableChannels[0];
            var channel2 = this.fixture.AvailableChannels[1];

            var productAvailableInChannel1 = this.fixture.FindProduct(p =>
                p.Key == this.fixture.KeyProductWithPrice50.valueOf());

            var productAvailableInChannel2 = this.fixture.FindProduct(p =>
                p.Key == this.fixture.KeyProductWithPrice30InChannel2.valueOf());

            var productAvailableInChannel3 = this.fixture.FindProduct(p =>
                p.Key == this.fixture.KeyProductWithPrice30InChannel3.valueOf());

            var productIds = new[]
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
            AssertEventually(() =>
            {
                var searchProductsOnStockResult =
                    client.ExecuteAsync(searchProductsOnStockInSpecificChannels).Result;
                Assert.Equal(2, searchProductsOnStockResult.Results.Count);

                Assert.DoesNotContain(searchProductsOnStockResult.Results, p => p.Id == productAvailableInChannel3.Id);
                Assert.Contains(searchProductsOnStockResult.Results, p => p.Id == productAvailableInChannel1.Id);
                Assert.Contains(searchProductsOnStockResult.Results, p => p.Id == productAvailableInChannel2.Id);
            });
        }


        [Fact]
        public void FilterByProductSearchKeywords()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var productWithoutSearchKeywords =
                this.fixture.FindProduct(p => p.SearchKeywords.Count == 0);
            var productWithSearchKeywords =
                this.fixture.FindProduct(p => p.SearchKeywords.Count > 0);
            Assert.NotNull(productWithSearchKeywords);
            Assert.NotNull(productWithoutSearchKeywords);
            var productIds = new[] {productWithoutSearchKeywords.Id, productWithSearchKeywords.Id};
            var searchKeywords = productWithSearchKeywords.SearchKeywords["en"];

            //Act (filter only the product which matches the specified state)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.FilterQuery(p => p.SearchKeywords["en"].Any(s => s.Text == searchKeywords[0].Text.valueOf()));

            //Assert
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == productWithSearchKeywords.Id);
            });
        }

        #endregion

        #region Facets

        [Fact]
        public void TestTermFacetWithProductCategories()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var categoryA = this.fixture.FindCategory(c => c.ExternalId == "A");
            var categoryB = this.fixture.FindCategory(c => c.ExternalId == "B");
            Assert.NotNull(categoryA);
            Assert.NotNull(categoryB);

            var productA = this.fixture.FindProduct(p =>
                p.Categories.Any(c => c.Id == categoryA.Id));
            var productB = this.fixture.FindProduct(p =>
                p.Categories.Any(c => c.Id == categoryB.Id));

            var productIds = new[] {productA.Id, productB.Id};

            //Act
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.TermFacet(p => p.Categories.Select(c => c.Id).FirstOrDefault(), isCountingProducts: true);

            //Assert
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Equal(2, searchResults.Results.Count);
                Assert.Single(searchResults.Facets);

                var termFacetResult = searchResults.Facets.FirstOrDefault().Value as TermFacetResult;
                Assert.NotNull(termFacetResult);
                Assert.Equal(2, termFacetResult.Terms.Count);

                Assert.Contains(termFacetResult.Terms,
                    t => t.Term.Equals(categoryA.Id) && t.Count == 1 && t.ProductCount == 1);
                Assert.Contains(termFacetResult.Terms,
                    t => t.Term.Equals(categoryB.Id) && t.Count == 1 && t.ProductCount == 1);
            });
        }

        [Fact]
        public void TestTermFacetNumberAttribute()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            var value30 = 30.0;
            var value50 = 50.0;
            //Arrange
            var product30 = this.fixture.FindProduct(p =>
                p.Key == this.fixture.KeyProductWithPrice30.valueOf());
            var product50 = this.fixture.FindProduct(p =>
                p.Key == this.fixture.KeyProductWithPrice50.valueOf());

            Assert.NotNull(product30);
            Assert.NotNull(product50);

            var productIds = new[] {product30.Id, product50.Id};

            //Act (filter only the product which have number attribute value = 30)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            //filter to the query results after facets have been calculated. so we expect termFacetResult with 2 terms
            searchRequest.Filter(p => p.Variants.Any(v =>
                v.Attributes.Any(a =>
                    a.Name == "number-attribute-name" && ((NumberAttribute) a).Value == value30)));

            searchRequest.TermFacet("GroupByNumberAttr", p => p.Variants.Any(v =>
                v.Attributes.Select(a => a.Name == "number-attribute-name").FirstOrDefault()));


            //Assert
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == product30.Id);

                Assert.Single(searchResults.Facets);


                var termFacetResult = searchResults.Facets["GroupByNumberAttr"] as TermFacetResult;
                Assert.NotNull(termFacetResult);
                Assert.Equal(2, termFacetResult.Terms.Count);

                Assert.Contains(termFacetResult.Terms, t => t.Term.Equals(value30.ToString("N1")) && t.Count == 1);
                Assert.Contains(termFacetResult.Terms, t => t.Term.Equals(value50.ToString("N1")) && t.Count == 1);
            });
        }

        [Fact]
        public void TestTermFacetCustomMoneyAttributeValue()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var product30 = this.fixture.FindProduct(p =>
                p.Key == this.fixture.KeyProductWithPrice30.valueOf());
            var product50 = this.fixture.FindProduct(p =>
                p.Key == this.fixture.KeyProductWithPrice50.valueOf());

            Assert.NotNull(product30);
            Assert.NotNull(product50);
            var productIds = new[] {product30.Id, product50.Id};

            //Act (filter only the product which have money custom attribute with specific value money30) and return facets with custom money attribute
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.Filter(p => p.Variants.Any(v => v.Attributes.Any(a =>
                a.Name == "money-attribute-name" &&
                ((MoneyAttribute) a).Value.CentAmount == TestingUtility.Money30.CentAmount.valueOf())));


            searchRequest.TermFacet("GroupByCustomMoney",
                p => p.Variants.Select(v =>
                        v.Attributes.Where(a => a.Name == "money-attribute-name")
                            .Select(a => ((MoneyAttribute) a).Value.CentAmount.ToString()).FirstOrDefault())
                    .FirstOrDefault(), isCountingProducts: true);

            //Assert
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == product30.Id);

                Assert.Single(searchResults.Facets);

                var termFacetResult = searchResults.Facets["GroupByCustomMoney"] as TermFacetResult;
                Assert.NotNull(termFacetResult);
                Assert.Equal(2, termFacetResult.Terms.Count);

                Assert.Contains(termFacetResult.Terms,
                    t => t.Term.Equals(TestingUtility.Money30.CentAmount.ToString()) && t.Count == 1 &&
                         t.ProductCount == 1);
                Assert.Contains(termFacetResult.Terms,
                    t => t.Term.Equals(TestingUtility.Money50.CentAmount.ToString()) && t.Count == 1 &&
                         t.ProductCount == 1);
            });
        }

        [Fact]
        public void TestTermFacetAvailableQuantityInChannels()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var channel1 = this.fixture.AvailableChannels[0];
            var channel2 = this.fixture.AvailableChannels[1];

            Assert.NotNull(channel1);
            Assert.NotNull(channel2);

            var productWithQuantity3InChannel1 = this.fixture.FindProduct(p =>
                p.Key == this.fixture.KeyProductWithPrice50.valueOf());

            var productWithQuantity6InChannel1 = this.fixture.FindProduct(p =>
                p.Key == this.fixture.KeyProductWithPrice70.valueOf());

            var productWithQuantity2InChannel2 = this.fixture.FindProduct(p =>
                p.Key == this.fixture.KeyProductWithPrice30InChannel2.valueOf());

            Assert.NotNull(productWithQuantity3InChannel1);
            Assert.NotNull(productWithQuantity6InChannel1);
            Assert.NotNull(productWithQuantity2InChannel2);

            var productIds = new[]
            {
                productWithQuantity3InChannel1.Id,
                productWithQuantity6InChannel1.Id,
                productWithQuantity2InChannel2.Id
            };

            //Act (Filter Products in Channel2) and Return Facets as Counts the ProductVariants for all occurring availableQuantity for the supply channel1
            var searchProductsRequest = new SearchProductProjectionsCommand();
            searchProductsRequest.SetStaged(true);
            searchProductsRequest.FilterQuery(p => p.Id.In(productIds));
            searchProductsRequest.Filter(p =>
                p.Variants.Any(v => v.Availability.IsOnStockInChannels(channel2.Id.valueOf())));

            searchProductsRequest.TermFacet("ProductsAvailabilityInChannel1", p =>
                p.Variants.Select(v => v.Availability.Channels[channel1.Id.valueOf()].AvailableQuantity)
                    .FirstOrDefault(), isCountingProducts: true);

            //Assert
            AssertEventually(() =>
            {
                var searchProductsResult =
                    client.ExecuteAsync(searchProductsRequest).Result;
                Assert.Single(searchProductsResult.Results);
                Assert.Contains(searchProductsResult.Results, p => p.Id == productWithQuantity2InChannel2.Id);

                Assert.Single(searchProductsResult.Facets);

                var termFacetResult = searchProductsResult.Facets["ProductsAvailabilityInChannel1"] as TermFacetResult;
                Assert.NotNull(termFacetResult);

                Assert.Equal(2, termFacetResult.Terms.Count);

                var expectedTerm1 = "3";
                var expectedTerm2 = "6";

                Assert.Contains(termFacetResult.Terms,
                    t => t.Term.Equals(expectedTerm1) && t.Count == 1 && t.ProductCount == 1);
                Assert.Contains(termFacetResult.Terms,
                    t => t.Term.Equals(expectedTerm2) && t.Count == 1 && t.ProductCount == 1);
            });
        }

        [Fact]
        public void TestRangeFacetByProductAvailabilityInChannel()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var productAvailableWithQuantity3 = this.fixture.FindProduct(p =>
                p.Key == this.fixture.KeyProductWithPrice50.valueOf());

            var productAvailableWithQuantity6 = this.fixture.FindProduct(p =>
                p.Key == this.fixture.KeyProductWithPrice70.valueOf());

            Assert.NotNull(productAvailableWithQuantity3);
            Assert.NotNull(productAvailableWithQuantity6);

            var channelId = this.fixture.AvailableChannels.FirstOrDefault()?.Id;

            var productIds = new[]
            {
                productAvailableWithQuantity3.Id,
                productAvailableWithQuantity6.Id
            };

            //Act (search Products with quantity >= 5) and return facets with 2 ranges (GreaterThan2,GreaterThan5)

            var searchProductsByAvailability = new SearchProductProjectionsCommand();
            searchProductsByAvailability.SetStaged(true);

            searchProductsByAvailability.FilterQuery(p => p.Id.In(productIds));
            searchProductsByAvailability.Filter(p =>
                p.Variants.Any(v => v.Availability.Channels[channelId].AvailableQuantity.Range(5, null)));

            searchProductsByAvailability.RangeFacet("GreaterThan2", p =>
                p.Variants.Any(v => v.Availability.Channels[channelId].AvailableQuantity.Range(2, null)));
            searchProductsByAvailability.RangeFacet("GreaterThan5", p =>
                p.Variants.Any(v => v.Availability.Channels[channelId].AvailableQuantity.Range(5, null)));

            //Assert
            AssertEventually(() =>
            {
                var searchProductsByAvailabilityResult =
                    client.ExecuteAsync(searchProductsByAvailability).Result;
                Assert.Single(searchProductsByAvailabilityResult.Results);
                Assert.Equal(productAvailableWithQuantity6.Id, searchProductsByAvailabilityResult.Results[0].Id);

                Assert.Equal(2, searchProductsByAvailabilityResult.Facets.Count);

                var greaterThan2RangeResult =
                    searchProductsByAvailabilityResult.Facets["GreaterThan2"] as RangeFacetResult;
                var greaterThan5RangeResult =
                    searchProductsByAvailabilityResult.Facets["GreaterThan5"] as RangeFacetResult;

                Assert.NotNull(greaterThan2RangeResult);
                Assert.NotNull(greaterThan5RangeResult);

                Assert.Single(greaterThan2RangeResult.Ranges);
                Assert.Equal(2, greaterThan2RangeResult.Ranges[0].Count);
                Assert.Equal(6, greaterThan2RangeResult.Ranges[0].Max);
                Assert.Equal(3, greaterThan2RangeResult.Ranges[0].Min);
            });
        }

        [Fact]
        public void TestRangeFacetByPriceCentAmount()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var product30 = this.fixture.FindProduct(p =>
                p.MasterVariant.Prices.Any(price =>
                    price.Value.CentAmount == TestingUtility.Euro30.Value.CentAmount));
            var product50 = this.fixture.FindProduct(p =>
                p.MasterVariant.Prices.Any(price =>
                    price.Value.CentAmount == TestingUtility.Euro50.Value.CentAmount));
            var product70 = this.fixture.FindProduct(p =>
                p.MasterVariant.Prices.Any(price =>
                    price.Value.CentAmount == TestingUtility.Euro70.Value.CentAmount));

            Assert.NotNull(product30);
            Assert.NotNull(product50);
            Assert.NotNull(product70);
            var productIds = new[] {product30.Id, product50.Id, product70.Id};

            //Act (filter products with specific price and return facet of 2 ranges for products with prices)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.Filter(p =>
                p.Variants.Any(v => v.Price.Value.CentAmount == TestingUtility.Euro30.Value.CentAmount.valueOf()));
            searchRequest.RangeFacet("ProductsBelow60",
                p => p.Variants.Any(v => v.Price.Value.CentAmount.Range(null, 60)));
            searchRequest.RangeFacet("ProductsFrom50To100",
                p => p.Variants.Any(v => v.Price.Value.CentAmount.Range(50, 100)));

            //Assert
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == product30.Id);

                Assert.Equal(2, searchResults.Facets.Count);
                var productsBelow60 = searchResults.Facets["ProductsBelow60"] as RangeFacetResult;
                var productsFrom50To100 = searchResults.Facets["ProductsFrom50To100"] as RangeFacetResult;

                Assert.NotNull(productsBelow60);
                Assert.Single(productsBelow60.Ranges);
                Assert.Equal(2, productsBelow60.Ranges[0].Count);
                Assert.Equal(TestingUtility.Euro30.Value.CentAmount, productsBelow60.Ranges[0].Min);
                Assert.Equal(TestingUtility.Euro50.Value.CentAmount, productsBelow60.Ranges[0].Max);


                Assert.NotNull(productsFrom50To100);
                Assert.Single(productsFrom50To100.Ranges);
                Assert.Equal(2, productsFrom50To100.Ranges[0].Count);
                Assert.Equal(TestingUtility.Euro50.Value.CentAmount, productsFrom50To100.Ranges[0].Min);
                Assert.Equal(TestingUtility.Euro70.Value.CentAmount, productsFrom50To100.Ranges[0].Max);
            });
        }


        [Fact]
        public void TestFilteredFacetWithProductCategories()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var categoryA = this.fixture.FindCategory(c => c.ExternalId == "A");
            var categoryA1 = this.fixture.FindCategory(c => c.ExternalId == "A1");
            var categoryB = this.fixture.FindCategory(c => c.ExternalId == "B");
            Assert.NotNull(categoryA);
            Assert.NotNull(categoryA1);
            Assert.NotNull(categoryB);

            var productA = this.fixture.FindProduct(p =>
                p.Categories.Any(c => c.Id == categoryA.Id));
            var productA1 = this.fixture.FindProduct(p =>
                p.Categories.Any(c => c.Id == categoryA1.Id));
            var productB = this.fixture.FindProduct(p =>
                p.Categories.Any(c => c.Id == categoryB.Id));

            Assert.NotNull(productA);
            Assert.NotNull(productA1);
            Assert.NotNull(productB);

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
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
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
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var product30 = this.fixture.FindProduct(p =>
                p.MasterVariant.Prices.Any(price =>
                    price.Value.CentAmount == TestingUtility.Euro30.Value.CentAmount));
            var product50 = this.fixture.FindProduct(p =>
                p.MasterVariant.Prices.Any(price =>
                    price.Value.CentAmount == TestingUtility.Euro50.Value.CentAmount));
            var product70 = this.fixture.FindProduct(p =>
                p.MasterVariant.Prices.Any(price =>
                    price.Value.CentAmount == TestingUtility.Euro70.Value.CentAmount));

            Assert.NotNull(product30);
            Assert.NotNull(product50);
            Assert.NotNull(product70);
            var productIds = new[] {product30.Id, product50.Id, product70.Id};

            //Act (filter products with specific price and return filtered facet for products with prices)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.Filter(p =>
                p.Variants.Any(v => v.Price.Value.CentAmount == TestingUtility.Euro30.Value.CentAmount.valueOf()));

            searchRequest.FilteredFacet("Products50",
                p => p.Variants.Any(v => v.Price.Value.CentAmount == TestingUtility.Euro50.Value.CentAmount.valueOf()),
                isCountingProducts: false);
            searchRequest.FilteredFacet("Products70",
                p => p.Variants.Any(v => v.Price.Value.CentAmount == TestingUtility.Euro70.Value.CentAmount.valueOf()),
                isCountingProducts: true);

            //Assert
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == product30.Id);

                Assert.Equal(2, searchResults.Facets.Count);
                var products50 = searchResults.Facets["Products50"] as FilteredFacetResult;
                var products70 = searchResults.Facets["Products70"] as FilteredFacetResult;

                Assert.NotNull(products50);
                Assert.True(products50.Count == 1);
                Assert.True(products50.ProductCount == 0);

                Assert.NotNull(products70);
                Assert.True(products70.Count == 1);
                Assert.True(products70.ProductCount == 1);
            });
        }

        [Fact]
        public void TestFilteredFacetNumberAttribute()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var value30 = 30.0;
            var value50 = 50.0;

            var product30 = this.fixture.FindProduct(p =>
                p.Key == this.fixture.KeyProductWithPrice30.valueOf());
            var product50 = this.fixture.FindProduct(p =>
                p.Key == this.fixture.KeyProductWithPrice50.valueOf());

            Assert.NotNull(product30);
            Assert.NotNull(product50);

            var productIds = new[] {product30.Id, product50.Id};

            //Act (filter only products which have custom number attribute with value 50, this will influence facet counts)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.FilterQuery(p => p.Variants.Any(v =>
                v.Attributes.Any(a =>
                    a.Name == "number-attribute-name" && ((NumberAttribute) a).Value == value50.valueOf())));

            searchRequest.FilteredFacet("ProductsByValue30Or50", p => p.Variants.Any(v =>
                v.Attributes.Any(a =>
                    a.Name == "number-attribute-name" && ((NumberAttribute) a).Value.In(new[] {value30, value50}))));


            //Assert
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Single(searchResults.Results);
                Assert.Contains(searchResults.Results, p => p.Id == product50.Id);

                Assert.Single(searchResults.Facets);

                var facetResult = searchResults.Facets["ProductsByValue30Or50"] as FilteredFacetResult;
                Assert.NotNull(facetResult);
                Assert.Equal(1, facetResult.Count); //because we're using filterQuery which will affect facet count
            });
        }

        #endregion

        #region Sort

        [Fact]
        public void SortByName()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            var product30 = this.fixture.FindProduct(p =>
                p.Key == this.fixture.KeyProductWithPrice30.valueOf()); //EntityForSearch_ProductWithPrice_30
            var product50 = this.fixture.FindProduct(p =>
                p.Key == this.fixture.KeyProductWithPrice50.valueOf()); //EntityForSearch_ProductWithPrice_50
            var product70 = this.fixture.FindProduct(p =>
                p.Key == this.fixture.KeyProductWithPrice70.valueOf()); //EntityForSearch_ProductWithPrice_70

            Assert.NotNull(product30);
            Assert.NotNull(product50);
            Assert.NotNull(product70);

            var productIds = new[] {product30.Id, product50.Id, product70.Id};

            //Act (sort Descending)
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.Sort(p => p.Name["en"], SortDirection.Descending);

            //Assert
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Equal(3, searchResults.Results.Count);

                Assert.Equal(product70.Id, searchResults.Results[0].Id);
                Assert.Equal(product50.Id, searchResults.Results[1].Id);
                Assert.Equal(product30.Id, searchResults.Results[2].Id);
            });
        }

        [Fact]
        public void SortByProductAvailability()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var productAvailableWithQuantity3 = this.fixture.FindProduct(p =>
                p.Key == this.fixture.KeyProductWithPrice50WithoutChannel.valueOf());

            var productAvailableWithQuantity6 = this.fixture.FindProduct(p =>
                p.Key == this.fixture.KeyProductWithPrice70WithoutChannel.valueOf());

            Assert.NotNull(productAvailableWithQuantity3);
            Assert.NotNull(productAvailableWithQuantity6);
            var productIds = new[]
            {
                productAvailableWithQuantity3.Id,
                productAvailableWithQuantity6.Id
            };

            //Act
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.Sort(p => p.Variants.Select(v => v.Availability.AvailableQuantity).FirstOrDefault(),
                SortDirection.Descending);

            //Assert
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Equal(2, searchResults.Results.Count);
                Assert.Equal(productAvailableWithQuantity6.Id, searchResults.Results[0].Id);
            });
        }

        [Fact]
        public void SortByProductAvailabilityWithSupplyChannel()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var productAvailableWithQuantity3 = this.fixture.FindProduct(p =>
                p.Key == this.fixture.KeyProductWithPrice50.valueOf());

            var productAvailableWithQuantity6 = this.fixture.FindProduct(p =>
                p.Key == this.fixture.KeyProductWithPrice70.valueOf());

            Assert.NotNull(productAvailableWithQuantity3);
            Assert.NotNull(productAvailableWithQuantity6);

            var channelId = this.fixture.AvailableChannels.FirstOrDefault()?.Id;

            var productIds = new[]
            {
                productAvailableWithQuantity3.Id,
                productAvailableWithQuantity6.Id
            };

            //Act
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.Sort(p =>
                p.Variants.Select(v => v.Availability.Channels[channelId].AvailableQuantity)
                    .FirstOrDefault(), SortDirection.Descending);

            //Assert
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Equal(2, searchResults.Results.Count);
                Assert.Equal(productAvailableWithQuantity6.Id, searchResults.Results[0].Id);
            });
        }

        [Fact]
        public void SortByProductRestockableInDaysInChannel()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var productRestockableInDays3 = this.fixture.FindProduct(p =>
                p.Key == this.fixture.KeyProductWithPrice50.valueOf());

            var productRestockableInDays6 = this.fixture.FindProduct(p =>
                p.Key == this.fixture.KeyProductWithPrice70.valueOf());

            var channelId = this.fixture.AvailableChannels.FirstOrDefault()?.Id;

            var productIds = new[]
            {
                productRestockableInDays3.Id,
                productRestockableInDays6.Id
            };

            //Act
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.Sort(
                p => p.Variants.Select(v => v.Availability.Channels[channelId].RestockableInDays).FirstOrDefault(),
                SortDirection.Descending);

            //Assert
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Equal(2, searchResults.Results.Count);
                Assert.Equal(productRestockableInDays6.Id, searchResults.Results[0].Id);
            });
        }

        [Fact]
        public void SortByReviewAverageRating()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var productWithAverage2 =
                this.fixture.FindProduct(p =>
                    p.Key == this.fixture.KeyProductWithPrice30.valueOf());

            var productWithAverage4 =
                this.fixture.FindProduct(p =>
                    p.Key == this.fixture.KeyProductWithPrice50.valueOf());

            var productWithAverage1 =
                this.fixture.FindProduct(p =>
                    p.Key == this.fixture.KeyProductWithPrice70.valueOf());

            Assert.NotNull(productWithAverage2);
            Assert.NotNull(productWithAverage4);
            Assert.NotNull(productWithAverage1);

            var productIds = new[] {productWithAverage2.Id, productWithAverage4.Id, productWithAverage1.Id};
            //Act
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.Sort(p => p.ReviewRatingStatistics.AverageRating, SortDirection.Descending);

            //Assert
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Equal(3, searchResults.Results.Count);

                Assert.Equal(productWithAverage4.Id, searchResults.Results[0].Id);
                Assert.Equal(4, searchResults.Results[0].ReviewRatingStatistics.AverageRating);

                Assert.Equal(productWithAverage2.Id, searchResults.Results[1].Id);
                Assert.Equal(2, searchResults.Results[1].ReviewRatingStatistics.AverageRating);

                Assert.Equal(productWithAverage1.Id, searchResults.Results[2].Id);
                Assert.Equal(1, searchResults.Results[2].ReviewRatingStatistics.AverageRating);
            });
        }

        [Fact]
        public void SortBySku()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var product1 = this.fixture.FindProduct(
                p => p.Key == this.fixture.KeyProductWithPrice50.valueOf());
            var product2 = this.fixture.FindProduct(
                p => p.Key == this.fixture.KeyProductWithPrice30.valueOf());
            var product3 = this.fixture.FindProduct(
                p => p.Key == this.fixture.KeyProductWithPrice70.valueOf());

            var productIds = new[] {product1.Id, product2.Id, product3.Id};
            //Act
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.Sort(p => p.Variants.Select(v => v.Sku).FirstOrDefault(), SortDirection.Ascending);

            //Assert
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Equal(3, searchResults.Results.Count);

                Assert.Equal(product2.Id, searchResults.Results[0].Id);
                Assert.Equal(product1.Id, searchResults.Results[1].Id);
                Assert.Equal(product3.Id, searchResults.Results[2].Id);
            });
        }

        [Fact]
        public void SortByCustomMoneyAttributeValue()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var product30 = this.fixture.FindProduct(p =>
                p.Key == this.fixture.KeyProductWithPrice30.valueOf());
            var product50 = this.fixture.FindProduct(p =>
                p.Key == this.fixture.KeyProductWithPrice50.valueOf());
            var product70 = this.fixture.FindProduct(p =>
                p.Key == this.fixture.KeyProductWithPrice70.valueOf());

            Assert.NotNull(product30);
            Assert.NotNull(product50);
            Assert.NotNull(product70);

            var productIds = new[] {product30.Id, product50.Id, product70.Id};

            //Act
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.Sort(
                p => p.Variants.Select(v =>
                        v.Attributes.Where(a => a.Name == "money-attribute-name")
                            .Select(a => ((MoneyAttribute) a).Value.CentAmount.ToString()).FirstOrDefault())
                    .FirstOrDefault(), SortDirection.Descending);

            //Assert
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Equal(3, searchResults.Results.Count);

                Assert.Equal(product70.Id, searchResults.Results[0].Id);
                Assert.Equal(product50.Id, searchResults.Results[1].Id);
                Assert.Equal(product30.Id, searchResults.Results[2].Id);
            });
        }

        [Fact]
        public void SortByEnumCustomAttributeLabel()
        {
            //check if products are indexed
            Assert.True(this.fixture.AvailableForSearch, NotReadyForSearchMessage);

            //Arrange
            var productWithEnum1 = this.fixture.FindProduct(p =>
                p.Key == this.fixture.KeyProductWithPrice30.valueOf());
            var productWithEnum2 = this.fixture.FindProduct(p =>
                p.Key == this.fixture.KeyProductWithPrice50.valueOf());
            var productWithEnum3 = this.fixture.FindProduct(p =>
                p.Key == this.fixture.KeyProductWithPrice70.valueOf());

            Assert.NotNull(productWithEnum1);
            Assert.NotNull(productWithEnum2);
            Assert.NotNull(productWithEnum3);
            var productIds = new[]
            {
                productWithEnum1.Id,
                productWithEnum2.Id,
                productWithEnum3.Id,
            };

            //Act
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetStaged(true);
            searchRequest.FilterQuery(p => p.Id.In(productIds));
            searchRequest.Sort(
                p => p.Variants.Select(v =>
                    v.Attributes.Where(a => a.Name == "enum-attribute-name")
                        .Select(a => ((EnumAttribute) a).Value.Label).FirstOrDefault()).FirstOrDefault(),
                SortDirection.Descending);

            //Assert
            AssertEventually(() =>
            {
                var searchResults = client.ExecuteAsync(searchRequest).Result;
                Assert.Equal(3, searchResults.Results.Count);

                Assert.Equal(productWithEnum3.Id, searchResults.Results[0].Id);
                Assert.Equal(productWithEnum2.Id, searchResults.Results[1].Id);
                Assert.Equal(productWithEnum1.Id, searchResults.Results[2].Id);
            });
        }

        #endregion
    }
}
