using System;
using System.Collections.Generic;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;

namespace commercetools.Sdk.HttpApi.IntegrationTests.Inventory
{
    public class InventoryFixture : ClientFixture, IDisposable
    {
        private ProductFixture productFixture;
        public List<InventoryEntry> InventoryEntries { get; }
        
        public InventoryFixture() : base()
        {
            this.InventoryEntries = new List<InventoryEntry>();
            this.productFixture = new ProductFixture();
        }
        
        public void Dispose()
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            this.InventoryEntries.Reverse();
            foreach (InventoryEntry inventoryEntry in this.InventoryEntries)
            {
                InventoryEntry deletedEntry = commerceToolsClient
                    .ExecuteAsync(new DeleteByIdCommand<InventoryEntry>(new Guid(inventoryEntry.Id), inventoryEntry.Version)).Result;
            }
            this.productFixture.Dispose();
        }
        
        public InventoryEntryDraft GetInventoryEntryDraft()
        {
            Product product = this.productFixture.CreateProduct(true);
            this.productFixture.ProductsToDelete.Add(product);
            
            InventoryEntryDraft inventoryEntryDraft = new InventoryEntryDraft();
            inventoryEntryDraft.Sku = product.MasterData.Current.MasterVariant.Sku;
            inventoryEntryDraft.QuantityOnStock = this.RandomInt(100, 1000);
            return inventoryEntryDraft;
        }

        public InventoryEntry CreateInventoryEntry()
        {
            return this.CreateInventoryEntry(this.GetInventoryEntryDraft());
        }

        public InventoryEntry CreateInventoryEntry(InventoryEntryDraft inventoryEntryDraft)
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            InventoryEntry inventoryEntry = commerceToolsClient.ExecuteAsync(new CreateCommand<InventoryEntry>(inventoryEntryDraft)).Result;
            return inventoryEntry;
        }
    }
}