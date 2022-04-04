using System;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.InventoryEntries;
using commercetools.Sdk.Domain.InventoryEntries.UpdateActions;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using Xunit;
using static commercetools.Sdk.IntegrationTests.Inventory.InventoryFixture;
using static commercetools.Sdk.IntegrationTests.Channels.ChannelsFixture;
using static commercetools.Sdk.IntegrationTests.Types.TypesFixture;

namespace commercetools.Sdk.IntegrationTests.Inventory
{
    [Collection("Integration Tests")]
    public class InventoryIntegrationTests
    {
        private readonly IClient client;

        public InventoryIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.client = serviceProviderFixture.GetService<IClient>();
        }

        [Fact]
        public async Task CreateInventoryEntry()
        {
            var sku = $"CreateInventoryEntry-{TestingUtility.RandomString()}";
            await WithInventoryEntry(
                client, inventoryEntryDraft => DefaultInventoryEntryDraftWithSku(inventoryEntryDraft, sku),
                inventoryEntry => { Assert.Equal(sku, inventoryEntry.Sku); });
        }

        [Fact]
        public async Task GetInventoryEntryById()
        {
            var sku = $"CreateInventoryEntry-{TestingUtility.RandomString()}";
            await WithInventoryEntry(
                client, inventoryEntryDraft => DefaultInventoryEntryDraftWithSku(inventoryEntryDraft, sku),
                async inventoryEntry =>
                {
                    var retrievedInventoryEntry = await client
                        .ExecuteAsync(inventoryEntry.ToIdResourceIdentifier().GetById());
                    Assert.Equal(sku, retrievedInventoryEntry.Sku);
                });
        }
        
        [Fact]
        public async Task GetInventoryEntryByKey()
        {
            var key = $"CreateInventoryEntry-{TestingUtility.RandomString()}";
            await WithInventoryEntry(
                client, inventoryEntryDraft => DefaultInventoryEntryDraftWithKey(inventoryEntryDraft, key),
                async inventoryEntry =>
                {
                    var retrievedInventoryEntry = await client
                        .ExecuteAsync(inventoryEntry.ToKeyResourceIdentifier().GetByKey());
                    Assert.Equal(key, retrievedInventoryEntry.Key);
                });
        }

        [Fact]
        public async Task QueryInventoryEntries()
        {
            var sku = $"QueryInventoryEntries-{TestingUtility.RandomString()}";
            await WithInventoryEntry(
                client, inventoryEntryDraft => DefaultInventoryEntryDraftWithSku(inventoryEntryDraft, sku),
                async inventoryEntry =>
                {
                    var queryCommand = new QueryCommand<InventoryEntry>();
                    queryCommand.Where(ie => ie.Sku == inventoryEntry.Sku.valueOf());
                    var returnedSet = await client.ExecuteAsync(queryCommand);
                    Assert.Single(returnedSet.Results);
                    Assert.Equal(sku, returnedSet.Results[0].Sku);
                });
        }

        [Fact]
        public async Task DeleteInventoryEntryById()
        {
            var sku = $"DeleteInventoryEntryById-{TestingUtility.RandomString()}";
            await WithInventoryEntry(
                client, inventoryEntryDraft => DefaultInventoryEntryDraftWithSku(inventoryEntryDraft, sku),
                async inventoryEntry =>
                {
                    await client.ExecuteAsync(inventoryEntry.DeleteById());
                    await Assert.ThrowsAsync<NotFoundException>(
                        () => client.ExecuteAsync(new GetByIdCommand<InventoryEntry>(inventoryEntry))
                    );
                });
        }
        
        [Fact]
        public async Task DeleteInventoryEntryByKey()
        {
            var key = $"DeleteInventoryEntryByKey-{TestingUtility.RandomString()}";
            await WithInventoryEntry(
                client, inventoryEntryDraft => DefaultInventoryEntryDraftWithKey(inventoryEntryDraft, key),
                async inventoryEntry =>
                {
                    await client.ExecuteAsync(inventoryEntry.DeleteByKey());
                    await Assert.ThrowsAsync<NotFoundException>(
                        () => client.ExecuteAsync(new GetByIdCommand<InventoryEntry>(inventoryEntry))
                    );
                });
        }

        #region UpdateActions

        [Fact]
        public async void UpdateInventoryEntryAddQuantity()
        {
            await WithUpdateableInventoryEntry(client, DefaultInventoryEntryDraft,
                async inventoryEntry =>
                {
                    var newAddedQuantity = TestingUtility.RandomInt(1, 10);
                    var action = new AddQuantityUpdateAction
                    {
                        Quantity = newAddedQuantity
                    };

                    var updatedInventoryEntry = await client
                        .ExecuteAsync(inventoryEntry.UpdateById(actions => actions.AddUpdate(action)));


                    Assert.Equal(inventoryEntry.QuantityOnStock + newAddedQuantity,
                        updatedInventoryEntry.QuantityOnStock);
                    Assert.Equal(inventoryEntry.AvailableQuantity + newAddedQuantity,
                        updatedInventoryEntry.AvailableQuantity);
                    return updatedInventoryEntry;
                });
        }
        
        [Fact]
        public async void UpdateInventoryEntrySetKey()
        {
            await WithUpdateableInventoryEntry(client, DefaultInventoryEntryDraft,
                async inventoryEntry =>
                {
                    var newKey = TestingUtility.RandomString();
                    var action = new SetKeyUpdateAction
                    {
                        Key = newKey
                    };

                    var updatedInventoryEntry = await client
                        .ExecuteAsync(inventoryEntry.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.Equal(newKey, updatedInventoryEntry.Key);
                    return updatedInventoryEntry;
                });
        }

        [Fact]
        public async void UpdateInventoryEntryRemoveQuantity()
        {
            await WithUpdateableInventoryEntry(client, DefaultInventoryEntryDraft,
                async inventoryEntry =>
                {
                    long newRemovedQuantity = TestingUtility.RandomInt(1, 10);
                    var action = new RemoveQuantityUpdateAction
                    {
                        Quantity = newRemovedQuantity
                    };

                    var updatedInventoryEntry = await client
                        .ExecuteAsync(inventoryEntry.UpdateById(actions => actions.AddUpdate(action)));


                    Assert.Equal(inventoryEntry.QuantityOnStock - newRemovedQuantity,
                        updatedInventoryEntry.QuantityOnStock);
                    Assert.Equal(inventoryEntry.AvailableQuantity - newRemovedQuantity,
                        updatedInventoryEntry.AvailableQuantity);
                    return updatedInventoryEntry;
                });
        }

        [Fact]
        public async void UpdateInventoryEntryChangeQuantity()
        {
            await WithUpdateableInventoryEntry(client, DefaultInventoryEntryDraft,
                async inventoryEntry =>
                {
                    var newQuantity = TestingUtility.RandomInt(100, 1000);
                    var action = new ChangeQuantityUpdateAction
                    {
                        Quantity = newQuantity
                    };

                    var updatedInventoryEntry = await client
                        .ExecuteAsync(inventoryEntry.UpdateById(actions => actions.AddUpdate(action)));


                    Assert.Equal(newQuantity, updatedInventoryEntry.QuantityOnStock);
                    Assert.Equal(newQuantity, updatedInventoryEntry.AvailableQuantity);
                    return updatedInventoryEntry;
                });
        }

        [Fact]
        public async void UpdateInventoryEntryChangeQuantityByKey()
        {
            var key = TestingUtility.RandomString();
            await WithUpdateableInventoryEntry(client, 
                draft => DefaultInventoryEntryDraftWithKey(draft, key),
                async inventoryEntry =>
                {
                    Assert.Equal(key, inventoryEntry.Key);
                    var newQuantity = TestingUtility.RandomInt(100, 1000);
                    var action = new ChangeQuantityUpdateAction
                    {
                        Quantity = newQuantity
                    };

                    var updatedInventoryEntry = await client
                        .ExecuteAsync(inventoryEntry.UpdateByKey(actions => actions.AddUpdate(action)));

                    Assert.Equal(key, updatedInventoryEntry.Key);
                    Assert.Equal(newQuantity, updatedInventoryEntry.QuantityOnStock);
                    Assert.Equal(newQuantity, updatedInventoryEntry.AvailableQuantity);
                    return updatedInventoryEntry;
                });
        }
        [Fact]
        public async void UpdateInventoryEntrySetRestockableInDays()
        {
            await WithUpdateableInventoryEntry(client, DefaultInventoryEntryDraft,
                async inventoryEntry =>
                {
                    var restockableInDays = TestingUtility.RandomInt(1, 100);
                    var action = new SetRestockableInDaysUpdateAction
                    {
                        RestockableInDays = restockableInDays
                    };

                    var updatedInventoryEntry = await client
                        .ExecuteAsync(inventoryEntry.UpdateById(actions => actions.AddUpdate(action)));


                    Assert.Equal(restockableInDays, updatedInventoryEntry.RestockableInDays);

                    SetRestockableInDaysUpdateAction action2 = new SetRestockableInDaysUpdateAction();
                    var updatedInventoryEntry2 = await client
                        .ExecuteAsync(updatedInventoryEntry.UpdateById(actions => actions.AddUpdate(action2)));
                    Assert.Null(updatedInventoryEntry2.RestockableInDays);

                    return updatedInventoryEntry2;
                });
        }

        [Fact]
        public async void UpdateInventoryEntrySetExpectedDelivery()
        {
            await WithUpdateableInventoryEntry(client, DefaultInventoryEntryDraft,
                async inventoryEntry =>
                {
                    var expectedDelivery = DateTime.Today.AddDays(TestingUtility.RandomInt(1, 100)).ToUniversalTime();

                    var action = new SetExpectedDeliveryUpdateAction
                    {
                        ExpectedDelivery = expectedDelivery
                    };

                    var updatedInventoryEntry = await client
                        .ExecuteAsync(inventoryEntry.UpdateById(actions => actions.AddUpdate(action)));


                    Assert.Equal(expectedDelivery, updatedInventoryEntry.ExpectedDelivery);
                    return updatedInventoryEntry;
                });
        }

        [Fact]
        public async void UpdateInventoryEntrySetSupplyChannel()
        {
            await WithChannel(client, async channel =>
            {
                await WithUpdateableInventoryEntry(client, DefaultInventoryEntryDraft,
                    async inventoryEntry =>
                    {
                        Assert.Null(inventoryEntry.SupplyChannel);
                        var action = new SetSupplyChannelUpdateAction
                        {
                            SupplyChannel = channel.ToKeyResourceIdentifier()
                        };

                        var updatedInventoryEntry = await client
                            .ExecuteAsync(inventoryEntry.UpdateById(actions => actions.AddUpdate(action)));

                        Assert.NotNull(updatedInventoryEntry.SupplyChannel);
                        Assert.Equal(channel.Id, updatedInventoryEntry.SupplyChannel.Id);
                        return updatedInventoryEntry;
                    });
            });
        }

        [Fact]
        public async void UpdateInventoryEntrySetCustomType()
        {
            var fields = CreateNewFields();
            await WithType(client, async type =>
            {
                await WithUpdateableInventoryEntry(client, DefaultInventoryEntryDraft,
                    async inventoryEntry =>
                    {
                        Assert.Null(inventoryEntry.Custom);
                        var action = new SetCustomTypeUpdateAction
                        {
                            Type = type.ToKeyResourceIdentifier(),
                            Fields = fields
                        };

                        var updatedInventoryEntry = await client
                            .ExecuteAsync(inventoryEntry.UpdateById(actions => actions.AddUpdate(action)));

                        Assert.NotNull(updatedInventoryEntry.Custom);
                        Assert.Equal(type.Id, updatedInventoryEntry.Custom.Type.Id);
                        return updatedInventoryEntry;
                    });
            });
        }

        [Fact]
        public async void UpdateInventoryEntrySetCustomField()
        {
            var fields = CreateNewFields();
            var newValue = TestingUtility.RandomString(10);
            await WithType(client, async type =>
            {
                await WithUpdateableInventoryEntry(client,
                    inventoryEntryDraft => DefaultInventoryEntryDraftWithCustomType(inventoryEntryDraft, type, fields),
                    async inventoryEntry =>
                    {
                        Assert.NotNull(inventoryEntry.Custom);
                        var action = new SetCustomFieldUpdateAction()
                        {
                            Name = "string-field", Value = newValue
                        };

                        var updatedInventoryEntry = await client
                            .ExecuteAsync(inventoryEntry.UpdateById(actions => actions.AddUpdate(action)));

                        Assert.NotNull(updatedInventoryEntry.Custom);
                        Assert.Equal(newValue, updatedInventoryEntry.Custom.Fields["string-field"]);
                        return updatedInventoryEntry;
                    });
            });
        }

        #endregion
    }
}