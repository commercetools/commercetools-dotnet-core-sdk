using System;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Stores;
using static commercetools.Sdk.IntegrationTests.GenericFixture;

namespace commercetools.Sdk.IntegrationTests.Stores
{
    public static class StoresFixture
    {
        #region DraftBuilds
        public static StoreDraft DefaultStoreDraft(StoreDraft storeDraft)
        {
            var randomInt = TestingUtility.RandomInt();
            storeDraft.Key = $"Key{randomInt}";
            storeDraft.Name = new LocalizedString() {{"en", $"Store_Name_{randomInt}"}};
            return storeDraft;
        }
        public static StoreDraft DefaultStoreDraftWithKey(StoreDraft draft, string key)
        {
            var storeDraft = DefaultStoreDraft(draft);
            storeDraft.Key = key;
            return storeDraft;
        }

        #endregion

        #region WithStore

        public static async Task WithStore( IClient client, Action<Store> func)
        {
            await With(client, new StoreDraft(), DefaultStoreDraft, func);
        }
        public static async Task WithStore( IClient client, Func<StoreDraft, StoreDraft> draftAction, Action<Store> func)
        {
            await With(client, new StoreDraft(), draftAction, func);
        }

        public static async Task WithStore( IClient client, Func<Store, Task> func)
        {
            await WithAsync(client, new StoreDraft(), DefaultStoreDraft, func);
        }
        public static async Task WithStore( IClient client, Func<StoreDraft, StoreDraft> draftAction, Func<Store, Task> func)
        {
            await WithAsync(client, new StoreDraft(), draftAction, func);
        }
        #endregion

        #region WithUpdateableStore

        public static async Task WithUpdateableStore(IClient client, Func<Store, Store> func)
        {
            await WithUpdateable(client, new StoreDraft(), DefaultStoreDraft, func);
        }

        public static async Task WithUpdateableStore(IClient client, Func<StoreDraft, StoreDraft> draftAction, Func<Store, Store> func)
        {
            await WithUpdateable(client, new StoreDraft(), draftAction, func);
        }

        public static async Task WithUpdateableStore(IClient client, Func<Store, Task<Store>> func)
        {
            await WithUpdateableAsync(client, new StoreDraft(), DefaultStoreDraft, func);
        }
        public static async Task WithUpdateableStore(IClient client, Func<StoreDraft, StoreDraft> draftAction, Func<Store, Task<Store>> func)
        {
            await WithUpdateableAsync(client, new StoreDraft(), draftAction, func);
        }

        #endregion
    }
}
