using System.Linq;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.Domain.ProductProjections;
using Xunit;
using static commercetools.Sdk.IntegrationTests.Products.ProductsFixture;
using static commercetools.Sdk.IntegrationTests.Categories.CategoriesFixture;
using static commercetools.Sdk.IntegrationTests.ProductTypes.ProductTypesFixture;

namespace commercetools.Sdk.IntegrationTests.ProductProjections
{
    [Collection("Integration Tests")]
    public class ProductProjectionsIntegrationTests
    {
        private readonly IClient client;

        public ProductProjectionsIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.client = serviceProviderFixture.GetService<IClient>();
        }

        [Fact]
        public async Task GetProductProjectionById()
        {
            await WithProduct(
                client, productDraft => DefaultProductDraftWithPublish(productDraft, true),
                async publishedProduct =>
                {
                    await WithProduct(
                        client, productDraft => DefaultProductDraftWithPublish(productDraft, false),
                        async stagedProduct =>
                        {
                            var stagedAdditionalParameters = new ProductProjectionAdditionalParameters
                            {
                                Staged = true
                            };
                            var stagedProductProjection = await client
                                .ExecuteAsync(new GetByIdCommand<ProductProjection>(stagedProduct.Id,
                                    stagedAdditionalParameters));
                            var publishedProductProjection = await client
                                .ExecuteAsync(new GetByIdCommand<ProductProjection>(publishedProduct.Id));

                            Assert.Equal(stagedProduct.Id, stagedProductProjection.Id);
                            Assert.True(stagedProductProjection.Published == false);
                            Assert.Equal(publishedProduct.Id, publishedProductProjection.Id);
                            Assert.True(publishedProductProjection.Published);
                        });
                });
        }

        [Fact]
        public async Task GetProductProjectionByKey()
        {
            await WithProduct(
                client, productDraft => DefaultProductDraftWithPublish(productDraft, true),
                async publishedProduct =>
                {
                    await WithProduct(
                        client, productDraft => DefaultProductDraftWithPublish(productDraft, false),
                        async stagedProduct =>
                        {
                            var stagedAdditionalParameters = new ProductProjectionAdditionalParameters
                            {
                                Staged = true
                            };
                            var stagedProductProjection = await client
                                .ExecuteAsync(new GetByKeyCommand<ProductProjection>(stagedProduct.Key,
                                    stagedAdditionalParameters));
                            var publishedProductProjection = await client
                                .ExecuteAsync(new GetByKeyCommand<ProductProjection>(publishedProduct.Key));

                            Assert.Equal(stagedProduct.Id, stagedProductProjection.Id);
                            Assert.True(stagedProductProjection.Published == false);
                            Assert.Equal(publishedProduct.Id, publishedProductProjection.Id);
                            Assert.True(publishedProductProjection.Published);
                        });
                });
        }

        [Fact]
        public async void QueryCurrentProductProjections()
        {
            await WithProduct(client,
                productDraft => DefaultProductDraftWithPublish(productDraft, true),
                async publishedProduct =>
                {
                    var queryCommand = new QueryCommand<ProductProjection>();
                    queryCommand.Where(p => p.Key == publishedProduct.Key.valueOf());
                    var returnedSet = await client.ExecuteAsync(queryCommand);
                    Assert.Single(returnedSet.Results);
                    Assert.Equal(publishedProduct.Key, returnedSet.Results[0].Key);
                });
        }

        [Fact]
        public async void QueryAndOffsetStagedProductProjections()
        {
            var productsCount = 3;
            await WithProductType(client, async productType =>
            {
                await WithListOfProducts(client, productDraft =>
                        DefaultProductDraftWithProductType(productDraft, productType), productsCount,
                    async products =>
                    {
                        Assert.Equal(productsCount, products.Count);

                        var stagedAdditionalParameters = new ProductProjectionAdditionalParameters
                        {
                            Staged = true
                        };

                        var queryCommand = new QueryCommand<ProductProjection>(stagedAdditionalParameters);
                        queryCommand.Where(productProjection =>
                            productProjection.ProductType.Id == productType.Id.valueOf());
                        queryCommand.SetOffset(2);
                        queryCommand.SetWithTotal(true);

                        var returnedSet = await client.ExecuteAsync(queryCommand);
                        Assert.Single(returnedSet.Results);
                        Assert.Equal(3, returnedSet.Total);
                    });
            });
        }
        
        [Fact]
        public async void QueryAndLimitStagedProductProjections()
        {
            var productsCount = 3;
            await WithProductType(client, async productType =>
            {
                await WithListOfProducts(client, productDraft =>
                        DefaultProductDraftWithProductType(productDraft, productType), productsCount,
                    async products =>
                    {
                        Assert.Equal(productsCount, products.Count);

                        var stagedAdditionalParameters = new ProductProjectionAdditionalParameters
                        {
                            Staged = true
                        };

                        var queryCommand = new QueryCommand<ProductProjection>(stagedAdditionalParameters);
                        queryCommand.Where(productProjection =>
                            productProjection.ProductType.Id == productType.Id.valueOf());
                        queryCommand.SetLimit(2);
                        queryCommand.SetWithTotal(true);

                        var returnedSet = await client.ExecuteAsync(queryCommand);
                        Assert.Equal(2,returnedSet.Results.Count);
                        Assert.Equal(3, returnedSet.Total);
                    });
            });
        }
        
        [Fact]
        public async void QueryAndSortStagedProductProjections()
        {
            var productsCount = 3;
            await WithProductType(client, async productType =>
            {
                await WithListOfProducts(client, productDraft =>
                        DefaultProductDraftWithProductType(productDraft, productType), productsCount,
                    async products =>
                    {
                        Assert.Equal(productsCount, products.Count);

                        var stagedAdditionalParameters = new ProductProjectionAdditionalParameters
                        {
                            Staged = true
                        };

                        var queryCommand = new QueryCommand<ProductProjection>(stagedAdditionalParameters);
                        queryCommand.Where(productProjection =>
                            productProjection.ProductType.Id == productType.Id.valueOf());
                        queryCommand.Sort(p=>p.Name["en"]);

                        var returnedSet = await client.ExecuteAsync(queryCommand);
                        var sortedList = returnedSet.Results.OrderBy(p => p.Name["en"]);
                        
                        Assert.True(sortedList.SequenceEqual(returnedSet.Results));
                    });
            });
        }
        
        [Fact]
        public async void QueryAndExpandParents()
        {
            await WithCategory(client, async category =>
            {
                await WithProductType(client, async productType =>
                {
                    await WithProduct(
                        client, draft =>
                        {
                            var productDraft = DefaultProductDraftWithProductType(draft, productType);
                            productDraft = DefaultProductDraftWithCategory(productDraft, category);
                            productDraft.Publish = true;
                            return productDraft;
                        },
                        async publishedProduct =>
                        {
                            Assert.True(publishedProduct.MasterData.Published);
                            Assert.NotNull(publishedProduct.ProductType);
                            Assert.NotEmpty(publishedProduct.MasterData.Current.Categories);
                            
                            var queryCommand = new QueryCommand<ProductProjection>();
                            queryCommand.Where(productProjection =>
                                productProjection.ProductType.Id == productType.Id.valueOf());
                            queryCommand.Expand(p => p.ProductType)
                                .Expand(p => p.Categories.ExpandAll());

                            var returnedSet = await client.ExecuteAsync(queryCommand);
                            Assert.NotNull(returnedSet.Results);
                            Assert.Contains(returnedSet.Results,
                                pp => pp.Id == publishedProduct.Id && pp.ProductType.Obj != null &&
                                      pp.Categories.Count == 1 &&
                                      pp.Categories[0].Obj != null);
                        });
                });
            });

        }
    }
}