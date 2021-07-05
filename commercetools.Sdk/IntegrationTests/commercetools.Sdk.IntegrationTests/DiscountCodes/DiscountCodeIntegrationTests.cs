using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.CartDiscounts;
using commercetools.Sdk.Domain.DiscountCodes;
using commercetools.Sdk.Domain.DiscountCodes.UpdateActions;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using Xunit;
using static commercetools.Sdk.IntegrationTests.DiscountCodes.DiscountCodesFixture;
using static commercetools.Sdk.IntegrationTests.CartDiscounts.CartDiscountsFixture;
using static commercetools.Sdk.IntegrationTests.Types.TypesFixture;
using SetDescriptionUpdateAction = commercetools.Sdk.Domain.DiscountCodes.UpdateActions.SetDescriptionUpdateAction;

namespace commercetools.Sdk.IntegrationTests.DiscountCodes
{
    [Collection("Integration Tests")]
    public class DiscountCodeIntegrationTests
    {
        private readonly IClient client;

        public DiscountCodeIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.client = serviceProviderFixture.GetService<IClient>();
        }

        [Fact]
        public async Task CreateDiscountCode()
        {
            var code = $"CreateDiscountCode-{TestingUtility.RandomString()}";
            await WithCartDiscount(client, DefaultCartDiscountDraftRequireDiscountCode, async cartDiscount =>
            {
                var cartDiscounts = new List<Reference<CartDiscount>>
                {
                    new Reference<CartDiscount>
                    {
                        Id = cartDiscount.Id
                    }
                };
                await WithDiscountCode(
                    client,
                    draft =>
                    {
                        var discountCodeDraft = DefaultDiscountCodeDraftWithCartDiscounts(draft, cartDiscounts);
                        discountCodeDraft = DefaultDiscountCodeDraftWithCode(discountCodeDraft, code);
                        return discountCodeDraft;
                    },
                    discountCode =>
                    {
                        Assert.Equal(code, discountCode.Code);
                    });
            });
        }

        [Fact]
        public async Task GetDiscountCodeById()
        {
            await WithDiscountCode(client, async discountCode =>
            {
                var retrievedDiscountCode = await client
                    .ExecuteAsync(new GetByIdCommand<DiscountCode>(discountCode.Id));
                Assert.Equal(discountCode.Code, retrievedDiscountCode.Code);
            });
        }


        [Fact]
        public async Task QueryDiscountCodes()
        {
            await WithDiscountCode(client, async discountCode =>
            {
                var queryCommand = new QueryCommand<DiscountCode>();
                queryCommand.Where(p => p.Code == discountCode.Code.valueOf());
                var returnedSet = await client.ExecuteAsync(queryCommand);
                Assert.Single(returnedSet.Results);
                Assert.Equal(discountCode.Code, returnedSet.Results[0].Code);
            });
        }

        [Fact]
        public async Task DeleteDiscountCodeById()
        {
            await WithDiscountCode(client, async discountCode =>
            {
                await client.ExecuteAsync(new DeleteByIdCommand<DiscountCode>(discountCode));
                await Assert.ThrowsAsync<NotFoundException>(
                    () => client.ExecuteAsync(new GetByIdCommand<DiscountCode>(discountCode))
                );
            });
        }


        #region UpdateActions

        [Fact]
        public async Task UpdateDiscountCodeSetName()
        {
            await WithUpdateableDiscountCode(client, async discountCode =>
            {
                var newName = new LocalizedString {{"en", TestingUtility.RandomString()}};
                var updateActions = new List<UpdateAction<DiscountCode>>();
                var action = new SetNameUpdateAction {Name = newName};
                updateActions.Add(action);

                var updatedDiscountCode = await client
                    .ExecuteAsync(new UpdateByIdCommand<DiscountCode>(discountCode, updateActions));

                Assert.Equal(newName["en"], updatedDiscountCode.Name["en"]);
                return updatedDiscountCode;
            });
        }

        [Fact]
        public async Task UpdateDiscountCodeSetDescription()
        {
            await WithUpdateableDiscountCode(client, async discountCode =>
            {
                var newDescription = new LocalizedString {{"en", TestingUtility.RandomString()}};
                var updateActions = new List<UpdateAction<DiscountCode>>();
                var action = new SetDescriptionUpdateAction {Description = newDescription};
                updateActions.Add(action);

                var updatedDiscountCode = await client
                    .ExecuteAsync(new UpdateByIdCommand<DiscountCode>(discountCode, updateActions));

                Assert.Equal(newDescription["en"], updatedDiscountCode.Description["en"]);
                return updatedDiscountCode;
            });
        }

        [Fact]
        public async Task UpdateDiscountCodeSetCartPredicate()
        {
            var newCartPredicate = "1 <> 1";
            await WithUpdateableDiscountCode(client, async discountCode =>
            {
                var updateActions = new List<UpdateAction<DiscountCode>>();
                var action = new SetCartPredicateUpdateAction {CartPredicate = newCartPredicate};
                updateActions.Add(action);

                var updatedDiscountCode = await client
                    .ExecuteAsync(new UpdateByIdCommand<DiscountCode>(discountCode, updateActions));

                Assert.Equal(newCartPredicate, updatedDiscountCode.CartPredicate);
                return updatedDiscountCode;
            });
        }

        [Fact]
        public async Task UpdateDiscountCodeSetMaxApplications()
        {
            var maxApplications = TestingUtility.RandomInt(1, 10);
            await WithUpdateableDiscountCode(client, async discountCode =>
            {
                var updateActions = new List<UpdateAction<DiscountCode>>();
                var action = new SetMaxApplicationsUpdateAction {MaxApplications = maxApplications};
                updateActions.Add(action);

                var updatedDiscountCode = await client
                    .ExecuteAsync(new UpdateByIdCommand<DiscountCode>(discountCode, updateActions));

                Assert.Equal(maxApplications, updatedDiscountCode.MaxApplications);
                return updatedDiscountCode;
            });
        }

        [Fact]
        public async Task UpdateDiscountCodeSetMaxApplicationsPerCustomer()
        {
            var maxApplications = TestingUtility.RandomInt(1, 10);
            await WithUpdateableDiscountCode(client, async discountCode =>
            {
                var updateActions = new List<UpdateAction<DiscountCode>>();
                var action = new SetMaxApplicationsPerCustomerUpdateAction
                {
                    MaxApplicationsPerCustomer = maxApplications
                };
                updateActions.Add(action);

                var updatedDiscountCode = await client
                    .ExecuteAsync(new UpdateByIdCommand<DiscountCode>(discountCode, updateActions));

                Assert.Equal(maxApplications, updatedDiscountCode.MaxApplicationsPerCustomer);
                return updatedDiscountCode;
            });
        }

        [Fact]
        public async Task UpdateDiscountCodeSetCustomType()
        {
            var fields = CreateNewFields();

            await WithType(client, async type =>
            {
                await WithUpdateableDiscountCode(client,
                    async discountCode =>
                    {
                        var updateActions = new List<UpdateAction<DiscountCode>>();
                        var setTypeAction = new SetCustomTypeUpdateAction
                        {
                            Type = type.ToKeyResourceIdentifier(),
                            Fields = fields
                        };
                        updateActions.Add(setTypeAction);

                        var updatedCartDiscount =
                            await client.ExecuteAsync(new UpdateByIdCommand<DiscountCode>(discountCode, updateActions));

                        Assert.Equal(type.Id, updatedCartDiscount.Custom.Type.Id);
                        return updatedCartDiscount;
                    });
            });
        }

        [Fact]
        public async Task UpdateDiscountCodeSetCustomField()
        {
            var fields = CreateNewFields();
            var newValue = TestingUtility.RandomString(10);

            await WithType(client, async type =>
            {
                await WithUpdateableDiscountCodeWithCustomType(client,
                    type, fields,
                    async discountCode =>
                    {
                        var updateActions = new List<UpdateAction<DiscountCode>>();
                        var setCustomFieldUpdateAction = new SetCustomFieldUpdateAction()
                        {
                            Name = "string-field", Value = newValue
                        };
                        updateActions.Add(setCustomFieldUpdateAction);

                        var updatedCartDiscount =
                            await client.ExecuteAsync(new UpdateByIdCommand<DiscountCode>(discountCode, updateActions));

                        Assert.Equal(newValue, updatedCartDiscount.Custom.Fields["string-field"]);
                        return updatedCartDiscount;
                    });
            });
        }

        [Fact]
        public async Task UpdateDiscountCodeChangeCartDiscounts()
        {
            await WithCartDiscount(client, DefaultCartDiscountDraftRequireDiscountCode, async cartDiscount1 =>
            {
                await WithCartDiscount(client, DefaultCartDiscountDraftRequireDiscountCode, async cartDiscount2 =>
                {
                    await WithUpdateableDiscountCode(client, async discountCode =>
                    {
                        Assert.Single(discountCode.CartDiscounts);
                        var cartDiscounts = new List<Reference<CartDiscount>>()
                        {
                            new Reference<CartDiscount> {Id = cartDiscount1.Id},
                            new Reference<CartDiscount> {Id = cartDiscount2.Id},
                        };
                        var updateActions = new List<UpdateAction<DiscountCode>>();
                        var action = new ChangeCartDiscountsUpdateAction
                        {
                            CartDiscounts = cartDiscounts
                        };
                        updateActions.Add(action);

                        var updatedDiscountCode = await client
                            .ExecuteAsync(new UpdateByIdCommand<DiscountCode>(discountCode, updateActions));

                        Assert.Equal(2, updatedDiscountCode.CartDiscounts.Count);
                        Assert.Contains(updatedDiscountCode.CartDiscounts,
                            cartDiscountRef => cartDiscountRef.Id == cartDiscount1.Id);
                        Assert.Contains(updatedDiscountCode.CartDiscounts,
                            cartDiscountRef => cartDiscountRef.Id == cartDiscount2.Id);
                        return updatedDiscountCode;
                    });
                });
            });
        }

        [Fact]
        public async Task UpdateDiscountCodeChangeGroups()
        {
            await WithUpdateableDiscountCode(client, async discountCode =>
            {
                var rand = TestingUtility.RandomInt();
                Assert.Empty(discountCode.Groups);
                var groups = new List<string>
                {
                    $"Group1_{rand}",
                    $"Group2_{rand}",
                };
                var updateActions = new List<UpdateAction<DiscountCode>>();
                var action = new ChangeGroupsUpdateAction {Groups = groups};
                updateActions.Add(action);

                var updatedDiscountCode = await client
                    .ExecuteAsync(new UpdateByIdCommand<DiscountCode>(discountCode, updateActions));

                Assert.Equal(2, updatedDiscountCode.Groups.Count);
                Assert.Equal(groups, updatedDiscountCode.Groups);
                return updatedDiscountCode;
            });
        }

        [Fact]
        public async Task UpdateDiscountCodeChangeIsActive()
        {
            await WithUpdateableDiscountCode(client, async discountCode =>
            {
                var isActive = !discountCode.IsActive;
                var updateActions = new List<UpdateAction<DiscountCode>>();
                var action = new ChangeIsActiveUpdateAction {IsActive = isActive};
                updateActions.Add(action);

                var updatedDiscountCode = await client
                    .ExecuteAsync(new UpdateByIdCommand<DiscountCode>(discountCode, updateActions));

                Assert.Equal(isActive, updatedDiscountCode.IsActive);
                return updatedDiscountCode;
            });
        }

        [Fact]
        public async Task UpdateDiscountCodeSetValidFrom()
        {
            await WithUpdateableDiscountCode(client, async discountCode =>
            {
                var newValidFrom = discountCode.ValidFrom.AddDays(1);
                var updateActions = new List<UpdateAction<DiscountCode>>();
                var action = new SetValidFromUpdateAction {ValidFrom = newValidFrom};
                updateActions.Add(action);

                var updatedDiscountCode = await client
                    .ExecuteAsync(new UpdateByIdCommand<DiscountCode>(discountCode, updateActions));

                Assert.Equal(newValidFrom, updatedDiscountCode.ValidFrom);
                return updatedDiscountCode;
            });
        }

        [Fact]
        public async Task UpdateDiscountCodeSetValidUntil()
        {
            await WithUpdateableDiscountCode(client, async discountCode =>
            {
                var newValidUntil = discountCode.ValidUntil.AddDays(1);
                var updateActions = new List<UpdateAction<DiscountCode>>();
                var action = new SetValidUntilUpdateAction {ValidUntil = newValidUntil};
                updateActions.Add(action);

                var updatedDiscountCode = await client
                    .ExecuteAsync(new UpdateByIdCommand<DiscountCode>(discountCode, updateActions));

                Assert.Equal(newValidUntil, updatedDiscountCode.ValidUntil);
                return updatedDiscountCode;
            });
        }

        [Fact]
        public async Task UpdateDiscountCodeSetValidFromAndUntil()
        {
            await WithUpdateableDiscountCode(client, async discountCode =>
            {
                var newValidFrom = discountCode.ValidFrom.AddDays(1);
                var newValidUntil = discountCode.ValidUntil.AddDays(1);

                var updateActions = new List<UpdateAction<DiscountCode>>();
                var action = new SetValidFromAndUntilUpdateAction
                {
                    ValidFrom = newValidFrom,
                    ValidUntil = newValidUntil
                };
                updateActions.Add(action);

                var updatedDiscountCode = await client
                    .ExecuteAsync(new UpdateByIdCommand<DiscountCode>(discountCode, updateActions));

                Assert.Equal(newValidFrom, updatedDiscountCode.ValidFrom);
                Assert.Equal(newValidUntil, updatedDiscountCode.ValidUntil);

                return updatedDiscountCode;
            });
        }

        #endregion
    }
}