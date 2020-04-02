using System.Collections.Generic;
using System.Linq;
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
using static commercetools.Sdk.IntegrationTests.Carts.CartsFixture;
using static commercetools.Sdk.IntegrationTests.Stores.StoresFixture;
using static commercetools.Sdk.IntegrationTests.Orders.OrdersFixture;
using static commercetools.Sdk.IntegrationTests.OrderEdits.OrderEditsFixture;


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
                await WithShippingMethod(
                    client,
                    shippingMethodDraft =>
                        DefaultShippingMethodDraftWithKeyWithTaxCategory(shippingMethodDraft, taxCategory.ToKeyResourceIdentifier(), key),
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
        public async Task GetShippingMethodsForLocation()
        {
            // get shippingMethods in USA and state New York 
            var usaLocation = new Location {Country = "US", State = "New York"};
            await WithShippingMethodInUsaZone(
                client,
                async shippingMethod =>
                {
                    Assert.NotNull(shippingMethod);
                    Assert.Single(shippingMethod.ZoneRates);
                    var expansions = new List<Expansion<ShippingMethod>>
                    {
                        new ReferenceExpansion<ShippingMethod>(sm => sm.ZoneRates.ExpandAll().Zone)
                    };

                    var command = new GetShippingMethodsForLocationCommand(usaLocation.Country, usaLocation.State, null, expansions);
                    var returnedSet = await client.ExecuteAsync(command);
                    Assert.Single(returnedSet.Results);
                    var returnedShippingMethod = returnedSet.Results[0];
                    Assert.Equal(shippingMethod.Key, returnedShippingMethod.Key);
                    
                    Assert.Single(returnedShippingMethod.ZoneRates);
                    var returnedZoneRate = returnedShippingMethod.ZoneRates[0];
                    
                    Assert.NotNull(returnedZoneRate.Zone.Obj);
                    Assert.Single(returnedZoneRate.Zone.Obj.Locations);
                    
                    var returnedLocation = returnedZoneRate.Zone.Obj.Locations[0];
                    Assert.Equal(usaLocation.Country, returnedLocation.Country);
                    Assert.Equal(usaLocation.State, returnedLocation.State);
                    
                });
        }

        [Fact]
        public async Task GetShippingMethodsForCart()
        {
            var shippingAddress = TestingUtility.GetRandomAddress();
            await WithShippingMethodWithZoneRateAndTaxCategory(client,
                DefaultShippingMethodDraft, shippingAddress,
                async shippingMethod =>
                {
                    await WithCart(client,
                        draft =>
                        {
                            var cartDraft = DefaultCartDraftWithShippingAddress(draft, shippingAddress);
                            cartDraft = DefaultCartDraftWithShippingMethod(cartDraft, shippingMethod);
                            return cartDraft;
                        },
                        async cart =>
                        {
                            Assert.NotNull(cart.ShippingInfo.ShippingMethod);
                            Assert.Equal(shippingMethod.Id,cart.ShippingInfo.ShippingMethod.Id);

                            var expansions = new List<Expansion<ShippingMethod>>
                            {
                                new ReferenceExpansion<ShippingMethod>(sm => sm.ZoneRates.ExpandAll().Zone)
                            };

                            var command = new GetShippingMethodsForCartCommand(cart.Id, expansions);
                            var returnedSet = await client.ExecuteAsync(command);
                            Assert.Single(returnedSet.Results);
                            var returnedShippingMethod = returnedSet.Results[0];
                            Assert.Equal(shippingMethod.Key, returnedShippingMethod.Key);
                    
                            Assert.Single(returnedShippingMethod.ZoneRates);
                            var returnedZoneRate = returnedShippingMethod.ZoneRates[0];
                    
                            Assert.True(returnedZoneRate.ShippingRates.Count(shippingRate => shippingRate.IsMatching) == 1);
                        });
                });
        }
        
        
        [Fact]
        public async Task GetShippingMethodsForOrderEdit()
        {
            await WithSimpleOrder(client, async order =>
            {
                Assert.NotNull(order.ShippingInfo.ShippingMethod);
                var shippingMethod =
                    await client.ExecuteAsync(order.ShippingInfo.ShippingMethod.GetById()
                        .Expand(sm => sm.ZoneRates.ExpandAll().Zone));
                
                Assert.NotNull(shippingMethod);
                Assert.Single(shippingMethod.ZoneRates);
                var zone = shippingMethod.ZoneRates[0].Zone;
                Assert.NotNull(zone);
                Assert.NotNull(zone.Obj);
                Assert.Single(zone.Obj.Locations);
                var location = zone.Obj.Locations[0];

                Assert.NotNull(location);
                
                await WithOrderEdit(client,
                    draft => DefaultOrderEditDraftWithStagedAction(draft, order),
                    async orderEdit =>
                    {
                        var command = new GetShippingMethodsForOrderEditCommand(orderEdit.Id, location.Country, location.State);
                        var returnedSet = await client.ExecuteAsync(command);
                        Assert.Single(returnedSet.Results);
                        var returnedShippingMethod = returnedSet.Results[0];
                        Assert.Equal(shippingMethod.Key, returnedShippingMethod.Key);
                    });
            });
        }
        
        
        [Fact]
        public async Task GetShippingMethodsForCartInStore()
        {
            await WithStore(client, async store =>
            {
                await WithCartWithSingleLineItem(client,2,
                    draft => DefaultCartDraftInStore(draft, store.ToKeyResourceIdentifier()),
                    async cart =>
                    {
                        Assert.NotNull(cart.Store);
                        Assert.Equal(store.Key, cart.Store.Key);
                        Assert.NotNull(cart.ShippingInfo.ShippingMethod);
                        
                        var shippingMethod =
                            await client.ExecuteAsync(cart.ShippingInfo.ShippingMethod.GetById());
                        
                        var expansions = new List<Expansion<ShippingMethod>>
                        {
                            new ReferenceExpansion<ShippingMethod>(sm => sm.ZoneRates.ExpandAll().Zone)
                        };

                        var command = new GetShippingMethodsForCartCommand(cart.Id, expansions).InStore(store);
                        var returnedSet = await client.ExecuteAsync(command);
                        Assert.Single(returnedSet.Results);
                        var returnedShippingMethod = returnedSet.Results[0];
                        Assert.Equal(shippingMethod.Key, returnedShippingMethod.Key);

                        Assert.Single(returnedShippingMethod.ZoneRates);
                        var returnedZoneRate = returnedShippingMethod.ZoneRates[0];

                        Assert.True(
                            returnedZoneRate.ShippingRates.Count(shippingRate => shippingRate.IsMatching) == 1);
                    });
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
                var action = new SetDescriptionUpdateAction
                {
                    Description = newDescription
                };
                updateActions.Add(action);

                var updatedShippingMethod = await client
                    .ExecuteAsync(new UpdateByIdCommand<ShippingMethod>(shippingMethod, updateActions));

                Assert.Equal(newDescription, updatedShippingMethod.Description);
                return updatedShippingMethod;
            });
        }
        
        [Fact]
        public async Task UpdateShippingMethodSetLocalizedDescription()
        {
            await WithUpdateableShippingMethod(client, async shippingMethod =>
            {
                var newDescription = TestingUtility.RandomString();
                var updateActions = new List<UpdateAction<ShippingMethod>>();
                var action = new SetLocalizedDescriptionUpdateAction
                {
                    LocalizedDescription = new LocalizedString() {{"en", newDescription}}
                };
                updateActions.Add(action);

                var updatedShippingMethod = await client
                    .ExecuteAsync(new UpdateByIdCommand<ShippingMethod>(shippingMethod, updateActions));

                Assert.Equal(newDescription, updatedShippingMethod.LocalizedDescription["en"]);
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
                    var updateActions = new List<UpdateAction<ShippingMethod>>();
                    var action = new ChangeTaxCategoryUpdateAction { TaxCategory = taxCategory.ToKeyResourceIdentifier() };
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
                        Zone = zone.ToKeyResourceIdentifier()
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
