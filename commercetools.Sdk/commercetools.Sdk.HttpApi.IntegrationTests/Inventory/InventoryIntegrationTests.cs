using System;
using System.Collections.Generic;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.Domain.Channels;
using commercetools.Sdk.Domain.InventoryEntries;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.Domain.Query;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using Xunit;
using SetCustomFieldUpdateAction = commercetools.Sdk.Domain.InventoryEntries.SetCustomFieldUpdateAction;
using Type = commercetools.Sdk.Domain.Type;

namespace commercetools.Sdk.HttpApi.IntegrationTests.Inventory
{
    [Collection("Integration Tests")]
    public class InventoryIntegrationTests : IClassFixture<InventoryFixture>
    {
        private readonly InventoryFixture inventoryFixture;

        public InventoryIntegrationTests(InventoryFixture inventoryFixture)
        {
            this.inventoryFixture = inventoryFixture;
        }

        [Fact]
        public void CreateInventoryEntry()
        {
            IClient commerceToolsClient = this.inventoryFixture.GetService<IClient>();
            InventoryEntryDraft inventoryEntryDraft = this.inventoryFixture.GetInventoryEntryDraft();
            InventoryEntry inventoryEntry = commerceToolsClient
                .ExecuteAsync(new CreateCommand<InventoryEntry>(inventoryEntryDraft)).Result;
            this.inventoryFixture.InventoryEntries.Add(inventoryEntry);
            Assert.Equal(inventoryEntryDraft.Sku, inventoryEntry.Sku);
        }

        [Fact]
        public void GetInventoryEntryById()
        {
            IClient commerceToolsClient = this.inventoryFixture.GetService<IClient>();
            InventoryEntry inventoryEntry = this.inventoryFixture.CreateInventoryEntry();
            this.inventoryFixture.InventoryEntries.Add(inventoryEntry);
            InventoryEntry retrievedInventoryEntry = commerceToolsClient
                .ExecuteAsync(new GetByIdCommand<InventoryEntry>(new Guid(inventoryEntry.Id))).Result;
            Assert.Equal(inventoryEntry.Id, retrievedInventoryEntry.Id);
        }

        [Fact]
        public async void DeleteInventoryEntryById()
        {
            IClient commerceToolsClient = this.inventoryFixture.GetService<IClient>();
            InventoryEntry inventoryEntry = this.inventoryFixture.CreateInventoryEntry();
            InventoryEntry retrievedInventoryEntry = commerceToolsClient
                .ExecuteAsync(
                    new DeleteByIdCommand<InventoryEntry>(new Guid(inventoryEntry.Id), inventoryEntry.Version))
                .Result;
            NotFoundException exception = await Assert.ThrowsAsync<NotFoundException>(() =>
                commerceToolsClient.ExecuteAsync(
                    new GetByIdCommand<InventoryEntry>(new Guid(retrievedInventoryEntry.Id))));
            Assert.Equal(404, exception.StatusCode);
        }

        [Fact]
        public void QueryInventoryEntries()
        {
            IClient commerceToolsClient = this.inventoryFixture.GetService<IClient>();
            InventoryEntry inventoryEntry = this.inventoryFixture.CreateInventoryEntry();
            this.inventoryFixture.InventoryEntries.Add(inventoryEntry);
            QueryPredicate<InventoryEntry> queryPredicate =
                new QueryPredicate<InventoryEntry>(ie => ie.Id == inventoryEntry.Id.valueOf());
            QueryCommand<InventoryEntry> queryCommand = new QueryCommand<InventoryEntry>();
            queryCommand.SetWhere(queryPredicate);
            PagedQueryResult<InventoryEntry> returnedSet = commerceToolsClient.ExecuteAsync(queryCommand).Result;
            Assert.Contains(returnedSet.Results, cd => cd.Id == inventoryEntry.Id);
        }

        #region UpdateActions

        [Fact]
        public void UpdateInventoryEntryByIdSetAddQuantity()
        {
            IClient commerceToolsClient = this.inventoryFixture.GetService<IClient>();
            InventoryEntry inventoryEntry = this.inventoryFixture.CreateInventoryEntry();

            long newAddedQuantity = this.inventoryFixture.RandomInt(1, 10);
            List<UpdateAction<InventoryEntry>> updateActions = new List<UpdateAction<InventoryEntry>>();

            AddQuantityUpdateAction addQuantityUpdateAction = new AddQuantityUpdateAction()
            {
                Quantity = newAddedQuantity
            };
            updateActions.Add(addQuantityUpdateAction);

            //Increments quantityOnStock and updates availableQuantity according to the new quantity
            InventoryEntry retrievedInventoryEntry = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<InventoryEntry>(new Guid(inventoryEntry.Id), inventoryEntry.Version,
                    updateActions)).Result;
            this.inventoryFixture.InventoryEntries.Add(retrievedInventoryEntry);

            Assert.Equal(inventoryEntry.Id, retrievedInventoryEntry.Id);
            Assert.Equal(inventoryEntry.QuantityOnStock + newAddedQuantity, retrievedInventoryEntry.QuantityOnStock);
            Assert.Equal(inventoryEntry.AvailableQuantity + newAddedQuantity,
                retrievedInventoryEntry.AvailableQuantity);
        }

        [Fact]
        public void UpdateInventoryEntryByIdSetRemoveQuantity()
        {
            IClient commerceToolsClient = this.inventoryFixture.GetService<IClient>();
            InventoryEntry inventoryEntry = this.inventoryFixture.CreateInventoryEntry();

            long newRemovedQuantity = this.inventoryFixture.RandomInt(1, 10);
            List<UpdateAction<InventoryEntry>> updateActions = new List<UpdateAction<InventoryEntry>>();

            RemoveQuantityUpdateAction removeQuantityUpdateAction = new RemoveQuantityUpdateAction()
            {
                Quantity = newRemovedQuantity
            };
            updateActions.Add(removeQuantityUpdateAction);

            //Decrements quantityOnStock and updates availableQuantity according to the new quantity
            InventoryEntry retrievedInventoryEntry = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<InventoryEntry>(new Guid(inventoryEntry.Id), inventoryEntry.Version,
                    updateActions)).Result;
            this.inventoryFixture.InventoryEntries.Add(retrievedInventoryEntry);

            Assert.Equal(inventoryEntry.Id, retrievedInventoryEntry.Id);
            Assert.Equal(inventoryEntry.QuantityOnStock - newRemovedQuantity, retrievedInventoryEntry.QuantityOnStock);
            Assert.Equal(inventoryEntry.AvailableQuantity - newRemovedQuantity,
                retrievedInventoryEntry.AvailableQuantity);
        }

        [Fact]
        public void UpdateInventoryEntryByIdChangeQuantity()
        {
            IClient commerceToolsClient = this.inventoryFixture.GetService<IClient>();
            InventoryEntry inventoryEntry = this.inventoryFixture.CreateInventoryEntry();

            long newQuantity = this.inventoryFixture.RandomInt(100, 1000);
            List<UpdateAction<InventoryEntry>> updateActions = new List<UpdateAction<InventoryEntry>>();

            ChangeQuantityUpdateAction changeQuantityUpdateAction = new ChangeQuantityUpdateAction()
            {
                Quantity = newQuantity
            };
            updateActions.Add(changeQuantityUpdateAction);

            //Sets quantityOnStock and updates availableQuantity according to the new quantity
            InventoryEntry retrievedInventoryEntry = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<InventoryEntry>(new Guid(inventoryEntry.Id), inventoryEntry.Version,
                    updateActions)).Result;
            this.inventoryFixture.InventoryEntries.Add(retrievedInventoryEntry);

            Assert.Equal(retrievedInventoryEntry.Id, inventoryEntry.Id);
            Assert.Equal(newQuantity, retrievedInventoryEntry.QuantityOnStock);
            Assert.Equal(newQuantity, retrievedInventoryEntry.AvailableQuantity);
        }

        [Fact]
        public void UpdateInventoryEntryByIdSetRestockableInDays()
        {
            IClient commerceToolsClient = this.inventoryFixture.GetService<IClient>();
            InventoryEntry inventoryEntry = this.inventoryFixture.CreateInventoryEntry();

            int newRestockableInDays = this.inventoryFixture.RandomInt(1, 100);
            List<UpdateAction<InventoryEntry>> updateActions = new List<UpdateAction<InventoryEntry>>();

            SetRestockableInDaysUpdateAction setRestockableInDaysUpdateAction = new SetRestockableInDaysUpdateAction()
            {
                RestockableInDays = newRestockableInDays
            };
            updateActions.Add(setRestockableInDaysUpdateAction);

            //Increments quantityOnStock and updates availableQuantity according to the new quantity
            InventoryEntry retrievedInventoryEntry = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<InventoryEntry>(new Guid(inventoryEntry.Id), inventoryEntry.Version,
                    updateActions)).Result;
            this.inventoryFixture.InventoryEntries.Add(retrievedInventoryEntry);

            Assert.Equal(retrievedInventoryEntry.Id, inventoryEntry.Id);
            Assert.Equal(newRestockableInDays, retrievedInventoryEntry.RestockableInDays);
        }

        [Fact]
        public void UpdateInventoryEntryByIdSetExpectedDelivery()
        {
            IClient commerceToolsClient = this.inventoryFixture.GetService<IClient>();
            InventoryEntry inventoryEntry = this.inventoryFixture.CreateInventoryEntry();

            DateTime newExpectedDelivery = DateTime.Today.AddDays(this.inventoryFixture.RandomInt(1, 100)).ToUniversalTime();
            List<UpdateAction<InventoryEntry>> updateActions = new List<UpdateAction<InventoryEntry>>();

            SetExpectedDeliveryUpdateAction setExpectedDeliveryUpdateAction = new SetExpectedDeliveryUpdateAction()
            {
                ExpectedDelivery = newExpectedDelivery
            };
            updateActions.Add(setExpectedDeliveryUpdateAction);

            InventoryEntry retrievedInventoryEntry = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<InventoryEntry>(new Guid(inventoryEntry.Id), inventoryEntry.Version,
                    updateActions)).Result;
            this.inventoryFixture.InventoryEntries.Add(retrievedInventoryEntry);

            Assert.Equal(retrievedInventoryEntry.Id, inventoryEntry.Id);
            Assert.Equal(newExpectedDelivery, retrievedInventoryEntry.ExpectedDelivery);
        }

        [Fact]
        public void UpdateInventoryEntryByIdSetSupplyChannel()
        {
            IClient commerceToolsClient = this.inventoryFixture.GetService<IClient>();
            InventoryEntry inventoryEntry = this.inventoryFixture.CreateInventoryEntry();

            Channel supplyChannel = this.inventoryFixture.channelFixture.CreateChannel();
            Reference<Channel> channelReference = new Reference<Channel>()
            {
                Id = supplyChannel.Id
            };

            List<UpdateAction<InventoryEntry>> updateActions = new List<UpdateAction<InventoryEntry>>();

            SetSupplyChannelUpdateAction setSupplyChannelUpdateAction = new SetSupplyChannelUpdateAction()
            {
                SupplyChannel = channelReference
            };
            updateActions.Add(setSupplyChannelUpdateAction);

            InventoryEntry retrievedInventoryEntry = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<InventoryEntry>(new Guid(inventoryEntry.Id), inventoryEntry.Version,
                    updateActions)).Result;
            this.inventoryFixture.InventoryEntries.Add(retrievedInventoryEntry);

            Assert.Equal(retrievedInventoryEntry.Id, inventoryEntry.Id);
            Assert.Equal(supplyChannel.Id, retrievedInventoryEntry.SupplyChannel.Id);
        }

        [Fact]
        public void UpdateInventoryEntryByIdSetCustomType()
        {
            IClient commerceToolsClient = this.inventoryFixture.GetService<IClient>();
            InventoryEntry inventoryEntry = this.inventoryFixture.CreateInventoryEntry();

            var type = this.inventoryFixture.CreateNewType();
            var fields = this.inventoryFixture.CreateNewFields();

            List<UpdateAction<InventoryEntry>> updateActions = new List<UpdateAction<InventoryEntry>>();

            SetCustomTypeUpdateAction setCustomTypeUpdateAction = new SetCustomTypeUpdateAction()
            {
                Type = new ResourceIdentifier<Type> {Key = type.Key}, Fields = fields
            };
            updateActions.Add(setCustomTypeUpdateAction);

            InventoryEntry retrievedInventoryEntry = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<InventoryEntry>(new Guid(inventoryEntry.Id), inventoryEntry.Version,
                    updateActions)).Result;
            this.inventoryFixture.InventoryEntries.Add(retrievedInventoryEntry);

            Assert.Equal(retrievedInventoryEntry.Id, inventoryEntry.Id);
            Assert.Equal(type.Id, retrievedInventoryEntry.Custom.Type.Id);
        }

        [Fact]
        public void UpdateInventoryEntryByIdSetCustomField()
        {
            IClient commerceToolsClient = this.inventoryFixture.GetService<IClient>();
            InventoryEntry inventoryEntry = this.inventoryFixture.CreateInventoryEntryWithCustomFields();

            List<UpdateAction<InventoryEntry>> updateActions = new List<UpdateAction<InventoryEntry>>();
            string newValue = this.inventoryFixture.RandomString(10);
            SetCustomFieldUpdateAction setCustomFieldUpdateAction = new SetCustomFieldUpdateAction()
            {
                Name = "string-field", Value = newValue
            };
            updateActions.Add(setCustomFieldUpdateAction);

            InventoryEntry retrievedInventoryEntry = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<InventoryEntry>(new Guid(inventoryEntry.Id), inventoryEntry.Version,
                    updateActions)).Result;
            this.inventoryFixture.InventoryEntries.Add(retrievedInventoryEntry);

            Assert.Equal(retrievedInventoryEntry.Id, inventoryEntry.Id);
            Assert.Equal(newValue, retrievedInventoryEntry.Custom.Fields["string-field"]);
        }

        #endregion
    }
}
