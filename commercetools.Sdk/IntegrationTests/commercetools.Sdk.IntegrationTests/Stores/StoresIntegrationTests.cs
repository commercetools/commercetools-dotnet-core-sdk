using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.Domain.Stores;
using commercetools.Sdk.Domain.Stores.UpdateActions;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using Xunit;
using static commercetools.Sdk.IntegrationTests.Stores.StoresFixture;

namespace commercetools.Sdk.IntegrationTests.Stores
{
    [Collection("Integration Tests")]
    public class StoresIntegrationTests
    {
        private readonly IClient client;

        public StoresIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.client = serviceProviderFixture.GetService<IClient>();
        }

        [Fact]
        public async Task CreateStore()
        {
            var key = $"CreateStore-{TestingUtility.RandomString()}";
            await WithStore(
                client, storeDraft => DefaultStoreDraftWithKey(storeDraft, key),
                store => { Assert.Equal(key, store.Key); });
        }

        [Fact]
        public async Task GetStoreById()
        {
            var key = $"GetStoreById-{TestingUtility.RandomString()}";
            await WithStore(
                client, storeDraft => DefaultStoreDraftWithKey(storeDraft, key),
                async store =>
                {
                    var retrievedStore = await client
                        .ExecuteAsync(new GetByIdCommand<Store>(store.Id));
                    Assert.Equal(key, retrievedStore.Key);
                });
        }

        [Fact]
        public async Task GetStoreByKey()
        {
            var key = $"GetStoreByKey-{TestingUtility.RandomString()}";
            await WithStore(
                client, storeDraft => DefaultStoreDraftWithKey(storeDraft, key),
                async store =>
                {
                    var retrievedStore = await client
                        .ExecuteAsync(new GetByKeyCommand<Store>(store.Key));
                    Assert.Equal(key, retrievedStore.Key);
                });
        }

        [Fact]
        public async Task QueryStores()
        {
            var key = $"QueryStores-{TestingUtility.RandomString()}";
            await WithStore(
                client, storeDraft => DefaultStoreDraftWithKey(storeDraft, key),
                async store =>
                {
                    var queryCommand = new QueryCommand<Store>();
                    queryCommand.Where(p => p.Key == store.Key.valueOf());
                    var returnedSet = await client.ExecuteAsync(queryCommand);
                    Assert.Single(returnedSet.Results);
                    Assert.Equal(key, returnedSet.Results[0].Key);
                });
        }

        [Fact]
        public async Task DeleteStoreById()
        {
            var key = $"DeleteStoreById-{TestingUtility.RandomString()}";
            await WithStore(
                client, storeDraft => DefaultStoreDraftWithKey(storeDraft, key),
                async store =>
                {
                    await client.ExecuteAsync(new DeleteByIdCommand<Store>(store));
                    await Assert.ThrowsAsync<NotFoundException>(
                        () => client.ExecuteAsync(new GetByIdCommand<Store>(store))
                    );
                });
        }

        [Fact]
        public async Task DeleteStoreByKey()
        {
            var key = $"DeleteStoreByKey-{TestingUtility.RandomString()}";
            await WithStore(
                client, storeDraft => DefaultStoreDraftWithKey(storeDraft, key),
                async store =>
                {
                    await client.ExecuteAsync(
                        new DeleteByKeyCommand<Store>(store.Key, store.Version));
                    await Assert.ThrowsAsync<NotFoundException>(
                        () => client.ExecuteAsync(new GetByIdCommand<Store>(store))
                    );
                });
        }


        #region UpdateActions

        [Fact]
        public async Task UpdateStoreByKeySetName()
        {
            await WithUpdateableStore(client, async store =>
            {
                var newName = new LocalizedString {{"en", TestingUtility.RandomString()}};
                var updateActions = new List<UpdateAction<Store>>();
                var action = new SetNameUpdateAction
                {
                    Name = newName
                };
                updateActions.Add(action);

                var updatedStore = await client
                    .ExecuteAsync(new UpdateByKeyCommand<Store>(store.Key, store.Version,
                        updateActions));

                Assert.Equal(newName, updatedStore.Name);
                return updatedStore;
            });
        }

        [Fact]
        public async Task UpdateStoreByIdSetName()
        {
            await WithUpdateableStore(client, async store =>
            {
                var newName = new LocalizedString {{"en", TestingUtility.RandomString()}};
                var updateActions = new List<UpdateAction<Store>>();
                var action = new SetNameUpdateAction
                {
                    Name = newName
                };
                updateActions.Add(action);

                var updatedStore = await client
                    .ExecuteAsync(new UpdateByIdCommand<Store>(store,
                        updateActions));

                Assert.Equal(newName, updatedStore.Name);
                return updatedStore;
            });
        }
        #endregion
    }
}
