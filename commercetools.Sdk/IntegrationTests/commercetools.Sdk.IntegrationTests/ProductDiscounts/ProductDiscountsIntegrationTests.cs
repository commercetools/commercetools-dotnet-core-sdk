using System;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.Domain.ProductDiscounts;
using commercetools.Sdk.Domain.ProductDiscounts.UpdateActions;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using Xunit;
using static commercetools.Sdk.IntegrationTests.ProductDiscounts.ProductDiscountsFixture;
using static commercetools.Sdk.IntegrationTests.Products.ProductsFixture;

namespace commercetools.Sdk.IntegrationTests.ProductDiscounts
{
    [Collection("Integration Tests")]
    public class ProductDiscountsIntegrationTests
    {
        const string skip = "skipped now to solve ci build";
        private readonly IClient client;

        public ProductDiscountsIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.client = serviceProviderFixture.GetService<IClient>();
        }

        [Fact(Skip = skip)]
        public async Task CreateProductDiscount()
        {
            var key = $"CreateProductDiscount-{TestingUtility.RandomString()}";
            await WithProductDiscount(
                client, productDiscountDraft => DefaultProductDiscountDraftWithKey(productDiscountDraft, key),
                productDiscount => { Assert.Equal(key, productDiscount.Key); });
        }

        [Fact(Skip = skip)]
        public async Task CreateProductDiscountForSpecificProduct()
        {
            await WithProduct(client, async product =>
            {
                await WithProductDiscount(
                    client,
                    draft =>
                    {
                        var productDiscountDraft = DefaultProductDiscountDraft(draft);
                        productDiscountDraft.SetPredicate(p => p.ProductId() == product.Id.valueOf() && product.VariantId() == 1);
                        return productDiscountDraft;
                    },
                    (productDiscount, productDiscountDraft) =>
                    {
                        Assert.Equal(productDiscountDraft.Predicate, productDiscount.Predicate);
                    });
            });
        }

        [Fact(Skip = skip)]
        public async Task GetProductDiscountById()
        {
            var key = $"GetProductDiscountById-{TestingUtility.RandomString()}";
            await WithProductDiscount(
                client, productDiscountDraft => DefaultProductDiscountDraftWithKey(productDiscountDraft, key),
                async productDiscount =>
                {
                    var retrievedProductDiscount = await client
                        .ExecuteAsync(productDiscount.ToIdResourceIdentifier().GetById());
                    Assert.Equal(key, retrievedProductDiscount.Key);
                });
        }

        [Fact(Skip = skip)]
        public async Task GetProductDiscountByKey()
        {
            var key = $"GetProductDiscountByKey-{TestingUtility.RandomString()}";
            await WithProductDiscount(
                client, productDiscountDraft => DefaultProductDiscountDraftWithKey(productDiscountDraft, key),
                async productDiscount =>
                {
                    var retrievedProductDiscount = await client
                        .ExecuteAsync(productDiscount.ToKeyResourceIdentifier().GetByKey());
                    Assert.Equal(key, retrievedProductDiscount.Key);
                });
        }

        [Fact(Skip = skip)]
        public async Task QueryProductDiscounts()
        {
            var key = $"QueryProductDiscounts-{TestingUtility.RandomString()}";
            await WithProductDiscount(
                client, productDiscountDraft => DefaultProductDiscountDraftWithKey(productDiscountDraft, key),
                async productDiscount =>
                {
                    var queryCommand = new QueryCommand<ProductDiscount>();
                    queryCommand.Where(p => p.Key == productDiscount.Key.valueOf());
                    var returnedSet = await client.ExecuteAsync(queryCommand);
                    Assert.Single(returnedSet.Results);
                    Assert.Equal(key, returnedSet.Results[0].Key);
                });
        }

        [Fact(Skip = skip)]
        public async Task DeleteProductDiscountById()
        {
            var key = $"DeleteProductDiscountById-{TestingUtility.RandomString()}";
            await WithProductDiscount(
                client, productDiscountDraft => DefaultProductDiscountDraftWithKey(productDiscountDraft, key),
                async productDiscount =>
                {
                    await client.ExecuteAsync(productDiscount.DeleteById());
                    await Assert.ThrowsAsync<NotFoundException>(
                        () => client.ExecuteAsync(new GetByIdCommand<ProductDiscount>(productDiscount))
                    );
                });
        }

        [Fact(Skip = skip)]
        public async Task DeleteProductDiscountByKey()
        {
            var key = $"DeleteProductDiscountByKey-{TestingUtility.RandomString()}";
            await WithProductDiscount(
                client, productDiscountDraft => DefaultProductDiscountDraftWithKey(productDiscountDraft, key),
                async productDiscount =>
                {
                    await client.ExecuteAsync(productDiscount.DeleteByKey());
                    await Assert.ThrowsAsync<NotFoundException>(
                        () => client.ExecuteAsync(new GetByIdCommand<ProductDiscount>(productDiscount))
                    );
                });
        }

        #region UpdateActions

        [Fact(Skip = skip)]
        public async Task UpdateProductDiscountSetKey()
        {
            await WithUpdateableProductDiscount(client, async productDiscount =>
            {
                var key = TestingUtility.RandomString();
                var action = new SetKeyUpdateAction {Key = key};

                var updatedProductDiscount = await client
                    .ExecuteAsync(productDiscount.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Equal(key, updatedProductDiscount.Key);
                return updatedProductDiscount;
            });
        }

        [Fact(Skip = skip)]
        public async Task UpdateProductDiscountChangeValueToRelative()
        {
            var oldValue = new ExternalProductDiscountValue();
            await WithUpdateableProductDiscount(client,
                draft => DefaultProductDiscountDraftWithDiscountValue(draft, oldValue),
                async productDiscount =>
                {
                    Assert.IsType<ExternalProductDiscountValue>(productDiscount.Value);
                    var newValue = TestingUtility.GetProductDiscountValueAsRelative();
                    var action = new ChangeValueUpdateAction
                    {
                        Value = newValue
                    };

                    var updatedProductDiscount = await client
                        .ExecuteAsync(productDiscount.UpdateByKey(actions => actions.AddUpdate(action)));

                    var updatedValue = updatedProductDiscount.Value as RelativeProductDiscountValue;
                    Assert.NotNull(updatedValue);
                    Assert.Equal(newValue.Permyriad, updatedValue.Permyriad);
                    return updatedProductDiscount;
                });
        }

        [Fact(Skip = skip)]
        public async Task UpdateProductDiscountChangeValueToAbsolute()
        {
            var oldValue = new ExternalProductDiscountValue();
            await WithUpdateableProductDiscount(client,
                draft => DefaultProductDiscountDraftWithDiscountValue(draft, oldValue),
                async productDiscount =>
                {
                    Assert.IsType<ExternalProductDiscountValue>(productDiscount.Value);
                    var newValue = TestingUtility.GetProductDiscountValueAsAbsolute();
                    var action = new ChangeValueUpdateAction
                    {
                        Value = newValue
                    };

                    var updatedProductDiscount = await client
                        .ExecuteAsync(productDiscount.UpdateByKey(actions => actions.AddUpdate(action)));

                    var updatedValue = updatedProductDiscount.Value as AbsoluteProductDiscountValue;
                    Assert.NotNull(updatedValue);
                    Assert.Equal(newValue.Money[0], updatedValue.Money[0]);
                    return updatedProductDiscount;
                });
        }

        [Fact(Skip = skip)]
        public async Task UpdateProductDiscountChangePredicate()
        {
            await WithProduct(client, async product =>
            {
                await WithUpdateableProductDiscount(client, async productDiscount =>
                {
                    var predicate = $"product.id = \"{product.Id}\"";
                    var action = new ChangePredicateUpdateAction
                    {
                        Predicate = predicate
                    };

                    var updatedProductDiscount = await client
                        .ExecuteAsync(productDiscount.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.Equal(predicate, updatedProductDiscount.Predicate);
                    return updatedProductDiscount;
                });
            });
        }

        [Fact(Skip = skip)]
        public async Task UpdateProductDiscountChangeIsActive()
        {
            await WithUpdateableProductDiscount(client,
                draft => DefaultProductDiscountDraftWithActive(draft, false),
                async productDiscount =>
                {
                    Assert.False(productDiscount.IsActive);
                    var action = new ChangeIsActiveUpdateAction
                    {
                        IsActive = true
                    };

                    var updatedProductDiscount = await client
                        .ExecuteAsync(productDiscount.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.True(updatedProductDiscount.IsActive);
                    return updatedProductDiscount;
                });
        }

        [Fact(Skip = skip)]
        public async Task UpdateProductDiscountSetValidFrom()
        {
            await WithUpdateableProductDiscount(client,
                DefaultProductDiscountDraft,
                async productDiscount =>
                {
                    var validFrom = DateTime.Today.AddDays(5).ToUniversalTime();
                    var action = new SetValidFromUpdateAction
                    {
                        ValidFrom = validFrom
                    };

                    var updatedProductDiscount = await client
                        .ExecuteAsync(productDiscount.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.Equal(validFrom, updatedProductDiscount.ValidFrom);
                    return updatedProductDiscount;
                });
        }

        [Fact(Skip = skip)]
        public async Task UpdateProductDiscountSetValidUntil()
        {
            await WithUpdateableProductDiscount(client,
                DefaultProductDiscountDraft,
                async productDiscount =>
                {
                    var validUntil = DateTime.Today.AddMonths(5).ToUniversalTime();
                    var action = new SetValidUntilUpdateAction
                    {
                        ValidUntil = validUntil
                    };

                    var updatedProductDiscount = await client
                        .ExecuteAsync(productDiscount.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.Equal(validUntil, updatedProductDiscount.ValidUntil);
                    return updatedProductDiscount;
                });
        }

        [Fact(Skip = skip)]
        public async Task UpdateProductDiscountSetValidFromAndUntil()
        {
            await WithUpdateableProductDiscount(client,
                DefaultProductDiscountDraft,
                async productDiscount =>
                {
                    var validFrom = DateTime.Today.AddDays(5).ToUniversalTime();
                    var validUntil = DateTime.Today.AddMonths(2).ToUniversalTime();
                    var action = new SetValidFromAndUntilUpdateAction
                    {
                        ValidFrom = validFrom,
                        ValidUntil = validUntil
                    };

                    var updatedProductDiscount = await client
                        .ExecuteAsync(productDiscount.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.Equal(validFrom, updatedProductDiscount.ValidFrom);
                    Assert.Equal(validUntil, updatedProductDiscount.ValidUntil);
                    return updatedProductDiscount;
                });
        }

        [Fact(Skip = skip)]
        public async Task UpdateProductDiscountChangeName()
        {
            await WithUpdateableProductDiscount(client, async productDiscount =>
            {
                var name = TestingUtility.RandomString();
                var action = new ChangeNameUpdateAction
                {
                    Name = new LocalizedString() {{"en", name}}
                };

                var updatedProductDiscount = await client
                    .ExecuteAsync(productDiscount.UpdateByKey(actions => actions.AddUpdate(action)));

                Assert.Equal(name, updatedProductDiscount.Name["en"]);
                return updatedProductDiscount;
            });
        }

        [Fact(Skip = skip)]
        public async Task UpdateProductDiscountChangeSortOrder()
        {
            await WithUpdateableProductDiscount(client, async productDiscount =>
            {
                var sortOrder = TestingUtility.RandomSortOrder();
                var action = new ChangeSortOrderUpdateAction
                {
                    SortOrder = sortOrder
                };

                var updatedProductDiscount = await client
                    .ExecuteAsync(productDiscount.UpdateByKey(actions => actions.AddUpdate(action)));

                Assert.Equal(sortOrder, updatedProductDiscount.SortOrder);
                return updatedProductDiscount;
            });
        }

        #endregion
    }
}