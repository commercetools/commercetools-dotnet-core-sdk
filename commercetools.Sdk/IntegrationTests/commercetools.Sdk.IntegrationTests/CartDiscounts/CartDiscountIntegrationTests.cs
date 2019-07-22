using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.CartDiscounts;
using commercetools.Sdk.Domain.CartDiscounts.UpdateActions;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using Xunit;
using static commercetools.Sdk.IntegrationTests.CartDiscounts.CartDiscountsFixture;
using static commercetools.Sdk.IntegrationTests.Types.TypesFixture;
using SetDescriptionUpdateAction = commercetools.Sdk.Domain.CartDiscounts.UpdateActions.SetDescriptionUpdateAction;

namespace commercetools.Sdk.IntegrationTests.CartDiscounts
{
    [Collection("Integration Tests")]
    public class CartDiscountIntegrationTests
    {
        private readonly IClient client;

        public CartDiscountIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.client = serviceProviderFixture.GetService<IClient>();
        }

        [Fact]
        public async Task CreateCartDiscount()
        {
            var key = $"CreateCartDiscount-{TestingUtility.RandomString()}";
            await WithCartDiscount(
                client, cartDiscountDraft => DefaultCartDiscountDraftWithKey(cartDiscountDraft, key),
                cartDiscount => { Assert.Equal(key, (string) cartDiscount.Key); });
        }


        [Fact]
        public async Task GetCartDiscountById()
        {
            var key = $"GetCartDiscountById-{TestingUtility.RandomString()}";
            await WithCartDiscount(
                client, cartDiscountDraft => DefaultCartDiscountDraftWithKey(cartDiscountDraft, key),
                async cartDiscount =>
                {
                    var retrievedCartDiscount = await client
                        .ExecuteAsync(new GetByIdCommand<CartDiscount>(cartDiscount.Id));
                    Assert.Equal(key, retrievedCartDiscount.Key);
                });
        }

        [Fact]
        public async Task GetCartDiscountByKey()
        {
            var key = $"GetCartDiscountByKey-{TestingUtility.RandomString()}";
            await WithCartDiscount(
                client, cartDiscountDraft => DefaultCartDiscountDraftWithKey(cartDiscountDraft, key),
                async cartDiscount =>
                {
                    var retrievedCartDiscount = await client
                        .ExecuteAsync(new GetByKeyCommand<CartDiscount>(cartDiscount.Key));
                    Assert.Equal(key, retrievedCartDiscount.Key);
                });
        }

        [Fact]
        public async Task QueryCartDiscounts()
        {
            var key = $"QueryCartDiscounts-{TestingUtility.RandomString()}";
            await WithCartDiscount(
                client, cartDiscountDraft => DefaultCartDiscountDraftWithKey(cartDiscountDraft, key),
                async cartDiscount =>
                {
                    var queryCommand = new QueryCommand<CartDiscount>();
                    queryCommand.Where(p => p.Key == cartDiscount.Key.valueOf());
                    var returnedSet = await client.ExecuteAsync(queryCommand);
                    Assert.Single(returnedSet.Results);
                    Assert.Equal(key, returnedSet.Results[0].Key);
                });
        }

        [Fact]
        public async Task DeleteCartDiscountById()
        {
            var key = $"DeleteCartDiscountById-{TestingUtility.RandomString()}";
            await WithCartDiscount(
                client, cartDiscountDraft => DefaultCartDiscountDraftWithKey(cartDiscountDraft, key),
                async cartDiscount =>
                {
                    await client.ExecuteAsync(new DeleteByIdCommand<CartDiscount>(cartDiscount));
                    await Assert.ThrowsAsync<NotFoundException>(
                        () => client.ExecuteAsync(new GetByIdCommand<CartDiscount>(cartDiscount))
                    );
                });
        }

        [Fact]
        public async Task DeleteCartDiscountByKey()
        {
            var key = $"DeleteCartDiscountByKey-{TestingUtility.RandomString()}";
            await WithCartDiscount(
                client, cartDiscountDraft => DefaultCartDiscountDraftWithKey(cartDiscountDraft, key),
                async cartDiscount =>
                {
                    await client.ExecuteAsync(new DeleteByKeyCommand<CartDiscount>(cartDiscount.Key, cartDiscount.Version));
                    await Assert.ThrowsAsync<NotFoundException>(
                        () => client.ExecuteAsync(new GetByIdCommand<CartDiscount>(cartDiscount))
                    );
                });
        }

        #region UpdateActions

        [Fact]
        public async Task UpdateCartDiscountSetKey()
        {
            var newKey = $"UpdateCartDiscountSetKey-{TestingUtility.RandomString()}";
            await WithUpdateableCartDiscount(client, async cartDiscount =>
            {
                var updateActions = new List<UpdateAction<CartDiscount>>();
                var setKeyAction = new SetKeyUpdateAction() { Key = newKey };
                updateActions.Add(setKeyAction);

                var updatedCartDiscount = await client
                    .ExecuteAsync(new UpdateByIdCommand<CartDiscount>(cartDiscount, updateActions));

                Assert.NotEqual(cartDiscount.Version, updatedCartDiscount.Version);
                Assert.Equal(newKey, updatedCartDiscount.Key);
                return updatedCartDiscount;
            });
        }

        [Fact]
        public async Task UpdateCartDiscountChangeValue()
        {
            var newAbsoluteValue = TestingUtility.GetRandomAbsoluteCartDiscountValue();

            await WithUpdateableCartDiscount(client, async cartDiscount =>
            {
                var updateActions = new List<UpdateAction<CartDiscount>>();
                var changeValueAction = new ChangeValueUpdateAction
                {
                    Value = newAbsoluteValue
                };
                updateActions.Add(changeValueAction);

                var updatedCartDiscount = await client
                    .ExecuteAsync(new UpdateByIdCommand<CartDiscount>(cartDiscount, updateActions));

                var retrievedCartDiscountAmount =
                    ((AbsoluteCartDiscountValue) updatedCartDiscount.Value).Money[0].CentAmount;

                Assert.NotEqual(cartDiscount.Version, updatedCartDiscount.Version);
                Assert.Equal(newAbsoluteValue.Money[0].CentAmount, retrievedCartDiscountAmount);
                return updatedCartDiscount;
            });
        }

        [Fact]
        public async Task UpdateCartDiscountChangeCartPredicate()
        {
            var newCartPredicate = "1 <> 1";

            await WithUpdateableCartDiscount(client, async cartDiscount =>
            {
                var updateActions = new List<UpdateAction<CartDiscount>>();
                var changeCartPredicateAction = new ChangeCartPredicateUpdateAction
                {
                    CartPredicate = newCartPredicate
                };
                updateActions.Add(changeCartPredicateAction);

                var updatedCartDiscount = await client
                    .ExecuteAsync(new UpdateByIdCommand<CartDiscount>(cartDiscount, updateActions));

                Assert.NotEqual(cartDiscount.Version, updatedCartDiscount.Version);
                Assert.Equal(newCartPredicate, updatedCartDiscount.CartPredicate);
                return updatedCartDiscount;
            });
        }

        [Fact]
        public async Task UpdateCartDiscountChangeTarget()
        {
            var newCartDiscountTarget = new LineItemsCartDiscountTarget() { Predicate = " 1 <> 1" };
            await WithUpdateableCartDiscount(client, async cartDiscount =>
            {
                var updateActions = new List<UpdateAction<CartDiscount>>();
                var changeTargetAction = new ChangeTargetUpdateAction
                {
                    Target = newCartDiscountTarget
                };
                updateActions.Add(changeTargetAction);

                var updatedCartDiscount = await client
                    .ExecuteAsync(new UpdateByIdCommand<CartDiscount>(cartDiscount, updateActions));

                var newTarget = updatedCartDiscount.Target as LineItemsCartDiscountTarget;

                Assert.NotNull(newTarget);
                Assert.Equal(newCartDiscountTarget.Predicate, newTarget.Predicate);
                return updatedCartDiscount;
            });
        }

        [Fact]
        public async Task UpdateCartDiscountChangeIsActive()
        {
            await WithUpdateableCartDiscount(client, async cartDiscount =>
            {
                var newIsActive = !cartDiscount.IsActive;
                var updateActions = new List<UpdateAction<CartDiscount>>();
                var changeIsActiveAction = new ChangeIsActiveUpdateAction { IsActive = newIsActive };
                updateActions.Add(changeIsActiveAction);

                var updatedCartDiscount = await client
                    .ExecuteAsync(new UpdateByIdCommand<CartDiscount>(cartDiscount, updateActions));

                Assert.Equal(newIsActive, updatedCartDiscount.IsActive);
                return updatedCartDiscount;
            });
        }

        [Fact]
        public async Task UpdateCartDiscountChangeName()
        {
            await WithUpdateableCartDiscount(client, async cartDiscount =>
            {
                var newName = new LocalizedString {{"en", TestingUtility.RandomString()}};
                var updateActions = new List<UpdateAction<CartDiscount>>();
                var action = new ChangeNameUpdateAction { Name = newName};
                updateActions.Add(action);

                var updatedCartDiscount = await client
                    .ExecuteAsync(new UpdateByIdCommand<CartDiscount>(cartDiscount, updateActions));

                Assert.Equal(newName["en"], updatedCartDiscount.Name["en"]);
                return updatedCartDiscount;
            });
        }

        [Fact]
        public async Task UpdateCartDiscountSetDescription()
        {
            await WithUpdateableCartDiscount(client, async cartDiscount =>
            {
                var newDescription = new LocalizedString {{"en", TestingUtility.RandomString()}};
                var updateActions = new List<UpdateAction<CartDiscount>>();
                var action = new SetDescriptionUpdateAction { Description = newDescription };
                updateActions.Add(action);

                var updatedCartDiscount = await client
                    .ExecuteAsync(new UpdateByIdCommand<CartDiscount>(cartDiscount, updateActions));

                Assert.Equal(newDescription["en"], updatedCartDiscount.Description["en"]);
                return updatedCartDiscount;
            });
        }

        [Fact]
        public async Task UpdateCartDiscountChangeSortOrder()
        {
            await WithUpdateableCartDiscount(client, async cartDiscount =>
            {
                var newSortOrder = TestingUtility.RandomSortOrder();
                var updateActions = new List<UpdateAction<CartDiscount>>();
                var action = new ChangeSortOrderUpdateAction { SortOrder = newSortOrder};
                updateActions.Add(action);

                var updatedCartDiscount = await client
                    .ExecuteAsync(new UpdateByIdCommand<CartDiscount>(cartDiscount, updateActions));

                Assert.Equal(newSortOrder, updatedCartDiscount.SortOrder);
                return updatedCartDiscount;
            });
        }

        [Fact]
        public async Task UpdateCartDiscountChangeRequiresDiscountCode()
        {
            await WithUpdateableCartDiscount(client, async cartDiscount =>
            {
                var newRequiresDiscountCode = !cartDiscount.RequiresDiscountCode;
                var updateActions = new List<UpdateAction<CartDiscount>>();
                var action = new ChangeRequiresDiscountCodeUpdateAction { RequiresDiscountCode = newRequiresDiscountCode};
                updateActions.Add(action);

                var updatedCartDiscount = await client
                    .ExecuteAsync(new UpdateByIdCommand<CartDiscount>(cartDiscount, updateActions));

                Assert.Equal(newRequiresDiscountCode, updatedCartDiscount.RequiresDiscountCode);
                return updatedCartDiscount;
            });
        }

        [Fact]
        public async Task UpdateCartDiscountSetValidFrom()
        {
            await WithUpdateableCartDiscount(client, async cartDiscount =>
            {
                var newValidFrom = cartDiscount.ValidFrom.AddDays(1);
                var updateActions = new List<UpdateAction<CartDiscount>>();
                var action = new SetValidFromUpdateAction { ValidFrom = newValidFrom };
                updateActions.Add(action);

                var updatedCartDiscount = await client
                    .ExecuteAsync(new UpdateByIdCommand<CartDiscount>(cartDiscount, updateActions));
                Assert.Equal(newValidFrom, updatedCartDiscount.ValidFrom);
                return updatedCartDiscount;
            });
        }

        [Fact]
        public async Task UpdateCartDiscountSetValidUntil()
        {
            await WithUpdateableCartDiscount(client, async cartDiscount =>
            {
                var newValidUntil = cartDiscount.ValidUntil.AddDays(1);
                var updateActions = new List<UpdateAction<CartDiscount>>();
                var action = new SetValidUntilUpdateAction { ValidUntil = newValidUntil };
                updateActions.Add(action);

                var updatedCartDiscount = await client
                    .ExecuteAsync(new UpdateByIdCommand<CartDiscount>(cartDiscount, updateActions));
                Assert.Equal(newValidUntil, updatedCartDiscount.ValidUntil);
                return updatedCartDiscount;
            });
        }

        [Fact]
        public async Task UpdateCartDiscountSetValidFromAndUntil()
        {
            await WithUpdateableCartDiscount(client, async cartDiscount =>
            {
                var newValidFrom = cartDiscount.ValidFrom.AddDays(1);
                var newValidUntil = cartDiscount.ValidUntil.AddDays(1);

                var updateActions = new List<UpdateAction<CartDiscount>>();
                var action = new SetValidFromAndUntilUpdateAction
                {
                    ValidFrom = newValidFrom,
                    ValidUntil = newValidUntil
                };
                updateActions.Add(action);

                var updatedCartDiscount = await client
                    .ExecuteAsync(new UpdateByIdCommand<CartDiscount>(cartDiscount, updateActions));

                Assert.Equal(newValidFrom, updatedCartDiscount.ValidFrom);
                Assert.Equal(newValidUntil, updatedCartDiscount.ValidUntil);
                return updatedCartDiscount;
            });
        }

        [Fact]
        public async Task UpdateCartDiscountChangeStackingMode()
        {
            await WithUpdateableCartDiscount(client, async cartDiscount =>
            {
                var newStackingMode = StackingMode.StopAfterThisDiscount;
                var updateActions = new List<UpdateAction<CartDiscount>>();
                var action = new ChangeStackingModeUpdateAction { StackingMode = newStackingMode};
                updateActions.Add(action);

                var updatedCartDiscount = await client
                    .ExecuteAsync(new UpdateByIdCommand<CartDiscount>(cartDiscount, updateActions));

                Assert.Equal(newStackingMode, updatedCartDiscount.StackingMode);
                return updatedCartDiscount;
            });
        }

        [Fact]
        public async Task UpdateCartDiscountSetCustomType()
        {
            var fields = CreateNewFields();

            await WithType(client, async type =>
            {
                await WithUpdateableCartDiscount(client,
                    async cartDiscount =>
                    {
                        var updateActions = new List<UpdateAction<CartDiscount>>();
                        var setTypeAction = new SetCustomTypeUpdateAction
                        {
                            Type = new ResourceIdentifier<Type>
                            {
                                Key = type.Key
                            },
                            Fields = fields
                        };
                        updateActions.Add(setTypeAction);

                        var updatedCartDiscount = await client.ExecuteAsync(new UpdateByIdCommand<CartDiscount>(cartDiscount, updateActions));

                        Assert.Equal(type.Id, updatedCartDiscount.Custom.Type.Id);
                        return updatedCartDiscount;
                    });
            });
        }

        [Fact]
        public async Task UpdateCartDiscountSetCustomField()
        {
            var fields = CreateNewFields();
            var newValue = TestingUtility.RandomString(10);

            await WithType(client, async type =>
            {
                await WithUpdateableCartDiscount(client,
                    cartDiscountDraft => DefaultCartDiscountDraftWithCustomType(cartDiscountDraft, type, fields),
                    async cartDiscount =>
                    {
                        var updateActions = new List<UpdateAction<CartDiscount>>();
                        var setCustomFieldUpdateAction = new SetCustomFieldUpdateAction()
                        {
                            Name = "string-field", Value = newValue
                        };
                        updateActions.Add(setCustomFieldUpdateAction);

                        var updatedCartDiscount = await client.ExecuteAsync(new UpdateByIdCommand<CartDiscount>(cartDiscount, updateActions));

                        Assert.Equal(newValue, updatedCartDiscount.Custom.Fields["string-field"]);
                        return updatedCartDiscount;
                    });
            });
        }

        #endregion
    }
}
