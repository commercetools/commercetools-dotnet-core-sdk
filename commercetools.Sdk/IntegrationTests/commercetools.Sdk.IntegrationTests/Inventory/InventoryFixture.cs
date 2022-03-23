using System;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.InventoryEntries;
using commercetools.Sdk.Domain.Predicates;
using static commercetools.Sdk.IntegrationTests.GenericFixture;
using Type = commercetools.Sdk.Domain.Types.Type;

namespace commercetools.Sdk.IntegrationTests.Inventory
{
    public class InventoryFixture
    {
        #region DraftBuilds

        public static InventoryEntryDraft DefaultInventoryEntryDraft(InventoryEntryDraft inventoryEntryDraft)
        {
            var randomInt = TestingUtility.RandomInt();
            inventoryEntryDraft.Sku = $"Sku{randomInt}";
            inventoryEntryDraft.QuantityOnStock = TestingUtility.RandomInt(100, 1000);
            inventoryEntryDraft.RestockableInDays = TestingUtility.RandomInt(1, 100);
            return inventoryEntryDraft;
        }
        public static InventoryEntryDraft DefaultInventoryEntryDraftWithKey(InventoryEntryDraft draft, string key)
        {
            var inventoryEntryDraft = DefaultInventoryEntryDraft(draft);
            inventoryEntryDraft.Key = key;
            return inventoryEntryDraft;
        }
        
        public static InventoryEntryDraft DefaultInventoryEntryDraftWithSku(InventoryEntryDraft draft, string sku)
        {
            var inventoryEntryDraft = DefaultInventoryEntryDraft(draft);
            inventoryEntryDraft.Sku = sku;
            return inventoryEntryDraft;
        }
        public static InventoryEntryDraft DefaultInventoryEntryDraftWithSpecificQuantity(InventoryEntryDraft draft, long quantity)
        {
            var inventoryEntryDraft = DefaultInventoryEntryDraft(draft);
            inventoryEntryDraft.QuantityOnStock = quantity;
            return inventoryEntryDraft;
        }
        public static InventoryEntryDraft DefaultInventoryEntryDraftWithCustomType(InventoryEntryDraft draft, Type type, Fields fields)
        {
            var customFieldsDraft = new CustomFieldsDraft
            {
                Type = type.ToKeyResourceIdentifier(),
                Fields = fields
            };

            var inventoryEntryDraft = DefaultInventoryEntryDraft(draft);
            inventoryEntryDraft.Custom = customFieldsDraft;

            return inventoryEntryDraft;
        }
        #endregion

        #region WithInventoryEntry

        public static async Task WithInventoryEntry( IClient client, Action<InventoryEntry> func)
        {
            await With(client, new InventoryEntryDraft(), DefaultInventoryEntryDraft, func);
        }
        public static async Task WithInventoryEntry( IClient client, Func<InventoryEntryDraft, InventoryEntryDraft> draftAction, Action<InventoryEntry> func)
        {
            await With(client, new InventoryEntryDraft(), draftAction, func);
        }

        public static async Task WithInventoryEntry( IClient client, Func<InventoryEntry, Task> func)
        {
            await WithAsync(client, new InventoryEntryDraft(), DefaultInventoryEntryDraft, func);
        }
        public static async Task WithInventoryEntry( IClient client, Func<InventoryEntryDraft, InventoryEntryDraft> draftAction, Func<InventoryEntry, Task> func)
        {
            await WithAsync(client, new InventoryEntryDraft(), draftAction, func);
        }
        #endregion

        #region WithUpdateableInventoryEntry

        public static async Task WithUpdateableInventoryEntry(IClient client, Func<InventoryEntry, InventoryEntry> func)
        {
            await WithUpdateable(client, new InventoryEntryDraft(), DefaultInventoryEntryDraft, func);
        }

        public static async Task WithUpdateableInventoryEntry(IClient client, Func<InventoryEntryDraft, InventoryEntryDraft> draftAction, Func<InventoryEntry, InventoryEntry> func)
        {
            await WithUpdateable(client, new InventoryEntryDraft(), draftAction, func);
        }

        public static async Task WithUpdateableInventoryEntry(IClient client, Func<InventoryEntry, Task<InventoryEntry>> func)
        {
            await WithUpdateableAsync(client, new InventoryEntryDraft(), DefaultInventoryEntryDraft, func);
        }
        public static async Task WithUpdateableInventoryEntry(IClient client, Func<InventoryEntryDraft, InventoryEntryDraft> draftAction, Func<InventoryEntry, Task<InventoryEntry>> func)
        {
            await WithUpdateableAsync(client, new InventoryEntryDraft(), draftAction, func);
        }

        #endregion
        
        public static InventoryEntry CreateOrRetrieveBySku(
            IClient client,
            string sku,
            InventoryEntryDraft draft,
            Func<InventoryEntryDraft, InventoryEntryDraft> draftAction
        )
        {
            InventoryEntry inventoryEntry = null;
            var queryCommand = new QueryCommand<InventoryEntry>();
            queryCommand.Where(ie => ie.Sku == sku.valueOf());
            var returnedSet = client.ExecuteAsync(queryCommand).Result;
            if (returnedSet.Count == 1) //it's exists
            {
                inventoryEntry = returnedSet.Results[0];
            }
            else
            {
                var buildDraft = draftAction.Invoke(draft);
                inventoryEntry = CreateResource(client, buildDraft).Result;
            }
            return inventoryEntry;
        }
    }
}