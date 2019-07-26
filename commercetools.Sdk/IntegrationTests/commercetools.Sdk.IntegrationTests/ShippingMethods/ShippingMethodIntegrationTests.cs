using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.Domain.Query;
using commercetools.Sdk.Domain.ShippingMethods;
using commercetools.Sdk.Domain.ShippingMethods.UpdateActions;
using commercetools.Sdk.Domain.TaxCategories;
using commercetools.Sdk.Domain.Zones;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using Xunit;
using static commercetools.Sdk.IntegrationTests.ShippingMethods.ShippingMethodsFixture;
using static commercetools.Sdk.IntegrationTests.TaxCategories.TaxCategoriesFixture;
using static commercetools.Sdk.IntegrationTests.Zones.ZonesFixture;
using SetDescriptionUpdateAction = commercetools.Sdk.Domain.ShippingMethods.UpdateActions.SetDescriptionUpdateAction;

namespace commercetools.Sdk.IntegrationTests.ShippingMethods
{
    [Collection("Integration Tests")]
    public class ShippingMethodIntegrationTests
    {
        private readonly IClient client;

        public ShippingMethodIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.client = serviceProviderFixture.GetService<IClient>();
        }

        [Fact]
        public async Task CreateShippingMethod()
        {
            var key = $"CreateShippingMethod-{TestingUtility.RandomString()}";
            await WithTaxCategory(client, async taxCategory =>
            {
                var taxCategoryRef = new ResourceIdentifier<TaxCategory>
                {
                    Key = taxCategory.Key
                };
                await WithShippingMethod(
                    client,
                    shippingMethodDraft =>
                        DefaultShippingMethodDraftWithKeyWithTaxCategory(shippingMethodDraft, taxCategoryRef, key),
                    shippingMethod =>
                    {
                        Assert.Equal(key, shippingMethod.Key);
                    });
            });
        }

        [Fact]
        public async Task GetShippingMethodById()
        {
            await WithShippingMethod(
                client,
                async shippingMethod =>
                {
                    var retrievedShippingMethod = await client
                        .ExecuteAsync(new GetByIdCommand<ShippingMethod>(shippingMethod.Id));
                    Assert.Equal(shippingMethod.Key, retrievedShippingMethod.Key);
                });
        }

        [Fact]
        public async Task GetShippingMethodByKey()
        {
            await WithShippingMethod(
                client,
                async shippingMethod =>
                {
                    var retrievedShippingMethod = await client
                        .ExecuteAsync(new GetByKeyCommand<ShippingMethod>(shippingMethod.Key));
                    Assert.Equal(shippingMethod.Key, retrievedShippingMethod.Key);
                });
        }

        [Fact]
        public async Task QueryShippingMethods()
        {
            await WithShippingMethod(
                client,
                async shippingMethod =>
                {
                    var queryCommand = new QueryCommand<ShippingMethod>();
                    queryCommand.Where(p => p.Key == shippingMethod.Key.valueOf());
                    var returnedSet = await client.ExecuteAsync(queryCommand);
                    Assert.Single(returnedSet.Results);
                    Assert.Equal(shippingMethod.Key, returnedSet.Results[0].Key);
                });
        }

        [Fact]
        public async Task DeleteShippingMethodById()
        {
            await WithShippingMethod(
                client,
                async shippingMethod =>
                {
                    await client.ExecuteAsync(new DeleteByIdCommand<ShippingMethod>(shippingMethod));
                    await Assert.ThrowsAsync<NotFoundException>(
                        () => client.ExecuteAsync(new GetByIdCommand<ShippingMethod>(shippingMethod))
                    );
                });
        }

        [Fact]
        public async Task DeleteShippingMethodByKey()
        {
            await WithShippingMethod(
                client,
                async shippingMethod =>
                {
                    await client.ExecuteAsync(
                        new DeleteByKeyCommand<ShippingMethod>(shippingMethod.Key, shippingMethod.Version));
                    await Assert.ThrowsAsync<NotFoundException>(
                        () => client.ExecuteAsync(new GetByIdCommand<ShippingMethod>(shippingMethod))
                    );
                });
        }


        #region UpdateActions

        [Fact]
        public async Task UpdateShippingMethodSetKey()
        {
            var newKey = $"UpdateShippingMethodSetKey-{TestingUtility.RandomString()}";
            await WithUpdateableShippingMethod(client, async shippingMethod =>
            {
                var updateActions = new List<UpdateAction<ShippingMethod>>();
                var action = new SetKeyUpdateAction() {Key = newKey};
                updateActions.Add(action);

                var updatedShippingMethod = await client
                    .ExecuteAsync(new UpdateByIdCommand<ShippingMethod>(shippingMethod, updateActions));

                Assert.Equal(newKey, updatedShippingMethod.Key);
                return updatedShippingMethod;
            });
        }

        [Fact]
        public async Task UpdateShippingMethodChangeName()
        {
            await WithUpdateableShippingMethod(client, async shippingMethod =>
            {
                var newName = TestingUtility.RandomString();
                var updateActions = new List<UpdateAction<ShippingMethod>>();
                var action = new ChangeNameUpdateAction { Name = newName };
                updateActions.Add(action);

                var updatedShippingMethod = await client
                    .ExecuteAsync(new UpdateByIdCommand<ShippingMethod>(shippingMethod, updateActions));

                Assert.Equal(newName, updatedShippingMethod.Name);
                return updatedShippingMethod;
            });
        }

        [Fact]
        public async Task UpdateShippingMethodSetDescription()
        {
            await WithUpdateableShippingMethod(client, async shippingMethod =>
            {
                var newDescription = TestingUtility.RandomString();
                var updateActions = new List<UpdateAction<ShippingMethod>>();
                var action = new SetDescriptionUpdateAction {  Description = newDescription };
                updateActions.Add(action);

                var updatedShippingMethod = await client
                    .ExecuteAsync(new UpdateByIdCommand<ShippingMethod>(shippingMethod, updateActions));

                Assert.Equal(newDescription, updatedShippingMethod.Description);
                return updatedShippingMethod;
            });
        }

        [Fact]
        public async Task UpdateShippingMethodChangeTaxCategory()
        {
            await WithTaxCategory(client, async taxCategory =>
            {
                await WithUpdateableShippingMethod(client, async shippingMethod =>
                {
                    Assert.NotEqual(taxCategory.Id, shippingMethod.TaxCategory.Id);
                    var taxCategoryReference = new ResourceIdentifier<TaxCategory>{ Key = taxCategory.Key };
                    var updateActions = new List<UpdateAction<ShippingMethod>>();
                    var action = new ChangeTaxCategoryUpdateAction { TaxCategory = taxCategoryReference };
                    updateActions.Add(action);

                    var updatedShippingMethod = await client
                        .ExecuteAsync(new UpdateByIdCommand<ShippingMethod>(shippingMethod, updateActions));

                    Assert.Equal(taxCategory.Id, updatedShippingMethod.TaxCategory.Id);
                    return updatedShippingMethod;
                });
            });
        }

        [Fact]
        public async Task UpdateShippingMethodChangeIsDefault()
        {
            await WithUpdateableShippingMethod(client, async shippingMethod =>
            {
                var isDefault = !shippingMethod.IsDefault;
                var updateActions = new List<UpdateAction<ShippingMethod>>();
                var action = new ChangeIsDefaultUpdateAction { IsDefault = isDefault };
                updateActions.Add(action);

                var updatedShippingMethod = await client
                    .ExecuteAsync(new UpdateByIdCommand<ShippingMethod>(shippingMethod, updateActions));

                Assert.Equal(isDefault, updatedShippingMethod.IsDefault);
                return updatedShippingMethod;
            });
        }

        [Fact]
        public async Task UpdateShippingMethodAddShippingRate()
        {
            await WithUpdateableShippingMethodWithZoneRate(client, async shippingMethod =>
            {
                Assert.Single(shippingMethod.ZoneRates);
                var zoneRate = shippingMethod.ZoneRates[0];
                Assert.Single(zoneRate.ShippingRates);

                var newRate = new ShippingRateDraft()
                {
                    Price = Money.FromDecimal("USD", 8),
                    FreeAbove = Money.FromDecimal("USD", 150)
                };
                var updateActions = new List<UpdateAction<ShippingMethod>>();
                var action = new AddShippingRateUpdateAction()
                {
                    Zone = zoneRate.Zone,
                    ShippingRate = newRate
                };
                updateActions.Add(action);

                var updatedShippingMethod = await client
                    .ExecuteAsync(new UpdateByIdCommand<ShippingMethod>(shippingMethod, updateActions));

                Assert.Single(updatedShippingMethod.ZoneRates);
                zoneRate = updatedShippingMethod.ZoneRates[0];

                Assert.Equal(2,zoneRate.ShippingRates.Count);
                var newShippingRate = zoneRate.ShippingRates[1];
                Assert.NotNull(newShippingRate);
                Assert.Equal(newRate.Price, newShippingRate.Price);
                Assert.Equal(newRate.FreeAbove, newShippingRate.FreeAbove);
                return updatedShippingMethod;
            });
        }

        [Fact]
        public async Task UpdateShippingMethodRemoveShippingRate()
        {
            await WithUpdateableShippingMethodWithZoneRate(client, async shippingMethod =>
            {
                Assert.Single(shippingMethod.ZoneRates);
                var zoneRate = shippingMethod.ZoneRates[0];
                Assert.Single(zoneRate.ShippingRates);
                var shippingRate = zoneRate.ShippingRates[0];

                var updateActions = new List<UpdateAction<ShippingMethod>>();
                var action = new RemoveShippingRateUpdateAction
                {
                    Zone = zoneRate.Zone,
                    ShippingRate = new ShippingRateDraft
                    {
                        Price = shippingRate.Price,
                        FreeAbove = shippingRate.FreeAbove
                    }
                };
                updateActions.Add(action);

                var updatedShippingMethod = await client
                    .ExecuteAsync(new UpdateByIdCommand<ShippingMethod>(shippingMethod, updateActions));

                Assert.Single(updatedShippingMethod.ZoneRates);
                Assert.Empty(updatedShippingMethod.ZoneRates[0].ShippingRates);
                return updatedShippingMethod;
            });
        }

        [Fact]
        public async Task UpdateShippingMethodAddZone()
        {
            await WithZone(client, async zone =>
            {
                await WithUpdateableShippingMethod(client, async shippingMethod =>
                {
                    Assert.Empty(shippingMethod.ZoneRates);
                    var updateActions = new List<UpdateAction<ShippingMethod>>();
                    var action = new AddZoneUpdateAction
                    {
                        Zone = new ResourceIdentifier<Zone> { Key = zone.Key }
                    };
                    updateActions.Add(action);

                    var updatedShippingMethod = await client
                        .ExecuteAsync(new UpdateByIdCommand<ShippingMethod>(shippingMethod, updateActions));

                    Assert.Single(updatedShippingMethod.ZoneRates);
                    Assert.Equal(zone.Id, updatedShippingMethod.ZoneRates[0].Zone.Id);
                    return updatedShippingMethod;
                });
            });
        }

        [Fact]
        public async Task UpdateShippingMethodRemoveZone()
        {
            await WithUpdateableShippingMethodWithZone(client, async shippingMethod =>
            {
                Assert.Single(shippingMethod.ZoneRates);
                var updateActions = new List<UpdateAction<ShippingMethod>>();
                var action = new RemoveZoneUpdateAction
                {
                    Zone = shippingMethod.ZoneRates[0].Zone
                };
                updateActions.Add(action);

                var updatedShippingMethod = await client
                    .ExecuteAsync(new UpdateByIdCommand<ShippingMethod>(shippingMethod, updateActions));

                Assert.Empty(updatedShippingMethod.ZoneRates);
                return updatedShippingMethod;
            });
        }

        [Fact]
        public async Task UpdateShippingMethodSetPredicate()
        {
            await WithUpdateableShippingMethod(client, async shippingMethod =>
            {
                var predicate = "1 <> 1";
                var updateActions = new List<UpdateAction<ShippingMethod>>();
                var action = new SetPredicateUpdateAction { Predicate = predicate };
                updateActions.Add(action);

                var updatedShippingMethod = await client
                    .ExecuteAsync(new UpdateByIdCommand<ShippingMethod>(shippingMethod, updateActions));

                Assert.Equal(predicate, updatedShippingMethod.Predicate);
                return updatedShippingMethod;
            });
        }

        #endregion
    }
}
