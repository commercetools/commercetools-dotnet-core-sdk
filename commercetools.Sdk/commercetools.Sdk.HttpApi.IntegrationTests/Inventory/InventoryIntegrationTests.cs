using System;
using System.Collections.Generic;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.Domain.InventoryEntries;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.Domain.Query;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using Xunit;

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
                .ExecuteAsync(new UpdateByIdCommand<InventoryEntry>(new Guid(inventoryEntry.Id), inventoryEntry.Version, updateActions)).Result;
            this.inventoryFixture.InventoryEntries.Add(retrievedInventoryEntry);
            
            Assert.Equal(inventoryEntry.Id, retrievedInventoryEntry.Id);
            Assert.Equal(inventoryEntry.QuantityOnStock + newAddedQuantity, retrievedInventoryEntry.QuantityOnStock);
            Assert.Equal(inventoryEntry.AvailableQuantity + newAddedQuantity, retrievedInventoryEntry.AvailableQuantity);
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
                .ExecuteAsync(new UpdateByIdCommand<InventoryEntry>(new Guid(inventoryEntry.Id), inventoryEntry.Version, updateActions)).Result;
            this.inventoryFixture.InventoryEntries.Add(retrievedInventoryEntry);
            
            Assert.Equal(inventoryEntry.Id, retrievedInventoryEntry.Id);
            Assert.Equal(inventoryEntry.QuantityOnStock - newRemovedQuantity, retrievedInventoryEntry.QuantityOnStock);
            Assert.Equal(inventoryEntry.AvailableQuantity - newRemovedQuantity, retrievedInventoryEntry.AvailableQuantity);
        }

        #endregion
    }
}