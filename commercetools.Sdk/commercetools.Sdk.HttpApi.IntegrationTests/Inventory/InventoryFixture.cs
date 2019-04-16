using System;
using System.Collections.Generic;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.HttpApi.IntegrationTests.Channels;
using commercetools.Sdk.HttpApi.IntegrationTests.Products;
using Type = commercetools.Sdk.Domain.Type;

namespace commercetools.Sdk.HttpApi.IntegrationTests.Inventory
{
    public class InventoryFixture : ClientFixture, IDisposable
    {
        private ProductFixture productFixture;
        public ChannelFixture channelFixture;
        private TypeFixture typeFixture;

        public List<InventoryEntry> InventoryEntries { get; }

        public InventoryFixture() : base()
        {
            this.InventoryEntries = new List<InventoryEntry>();
            this.productFixture = new ProductFixture();
            this.channelFixture = new ChannelFixture();
            this.typeFixture = new TypeFixture();
        }

        public void Dispose()
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            this.InventoryEntries.Reverse();
            foreach (InventoryEntry inventoryEntry in this.InventoryEntries)
            {
                InventoryEntry deletedEntry = commerceToolsClient
                    .ExecuteAsync(new DeleteByIdCommand<InventoryEntry>(new Guid(inventoryEntry.Id),
                        inventoryEntry.Version)).Result;
            }

            this.productFixture.Dispose();
            this.channelFixture.Dispose();
            this.typeFixture.Dispose();
        }

        public InventoryEntryDraft GetInventoryEntryDraft()
        {
            Product product = this.productFixture.CreateProduct(true);
            this.productFixture.ProductsToDelete.Add(product);

            InventoryEntryDraft inventoryEntryDraft = new InventoryEntryDraft();
            inventoryEntryDraft.Sku = product.MasterData.Current.MasterVariant.Sku;
            inventoryEntryDraft.QuantityOnStock = TestingUtility.RandomInt(100, 1000);
            inventoryEntryDraft.RestockableInDays = TestingUtility.RandomInt(1, 100);
            return inventoryEntryDraft;
        }

        public InventoryEntry CreateInventoryEntry()
        {
            return this.CreateInventoryEntry(this.GetInventoryEntryDraft());
        }

        public InventoryEntry CreateInventoryEntry(InventoryEntryDraft inventoryEntryDraft)
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            InventoryEntry inventoryEntry = commerceToolsClient
                .ExecuteAsync(new CreateCommand<InventoryEntry>(inventoryEntryDraft)).Result;
            return inventoryEntry;
        }

        public InventoryEntry CreateInventoryEntryWithCustomFields()
        {
            InventoryEntryDraft draft = this.CreateInventoryEntryDraftWithCustomFields();
            IClient commerceToolsClient = this.GetService<IClient>();
            InventoryEntry inventoryEntry =
                commerceToolsClient.ExecuteAsync(new CreateCommand<InventoryEntry>(draft)).Result;
            return inventoryEntry;
        }

        public InventoryEntryDraft CreateInventoryEntryDraftWithCustomFields()
        {
            InventoryEntryDraft draft = this.GetInventoryEntryDraft();
            CustomFieldsDraft customFieldsDraft = new CustomFieldsDraft();
            Type type = this.typeFixture.CreateType();
            this.typeFixture.TypesToDelete.Add(type);
            customFieldsDraft.Type = new ResourceIdentifier<Type> {Key = type.Key};
            customFieldsDraft.Fields = this.CreateNewFields();
            draft.Custom = customFieldsDraft;
            return draft;
        }

        public Fields CreateNewFields()
        {
            Fields fields = this.typeFixture.CreateNewFields();
            return fields;
        }

        public Type CreateNewType()
        {
            Type type = this.typeFixture.CreateType();
            this.typeFixture.TypesToDelete.Add(type);
            return type;
        }
    }
}
