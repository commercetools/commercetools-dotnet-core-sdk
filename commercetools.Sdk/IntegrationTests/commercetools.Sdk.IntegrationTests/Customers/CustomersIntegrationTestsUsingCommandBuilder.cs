using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.Domain.Customers;
using commercetools.Sdk.Domain.Customers.UpdateActions;
using commercetools.Sdk.Domain.Stores;
using commercetools.Sdk.HttpApi.CommandBuilders;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using Xunit;
using static commercetools.Sdk.IntegrationTests.Customers.CustomersFixture;
using static commercetools.Sdk.IntegrationTests.CustomerGroups.CustomerGroupsFixture;
using static commercetools.Sdk.IntegrationTests.Types.TypesFixture;
using static commercetools.Sdk.IntegrationTests.Projects.ProjectFixture;
using static commercetools.Sdk.IntegrationTests.Stores.StoresFixture;
using static commercetools.Sdk.IntegrationTests.GenericFixture;

namespace commercetools.Sdk.IntegrationTests.Customers
{
    [Collection("Integration Tests")]
    public class CustomersIntegrationTestsUsingCommandBuilder
    {
        private readonly IClient client;

        public CustomersIntegrationTestsUsingCommandBuilder(ServiceProviderFixture serviceProviderFixture)
        {
            this.client = serviceProviderFixture.GetService<IClient>();
        }


        [Fact]
        public async Task GetCustomerById()
        {
            //Get Global Customer By Id
            var key = $"GetCustomerById-{TestingUtility.RandomString()}";
            await WithCustomer(
                client, customerDraft => DefaultCustomerDraftWithKey(customerDraft, key),
                async customer =>
                {
                    var retrievedCustomer = await client
                        .Builder()
                        .Customers()
                        .GetById(customer.Id)
                        .ExecuteAsync();
                    Assert.Equal(key, retrievedCustomer.Key);
                });
        }

        [Fact]
        public async Task GetCustomerInStoreById()
        {
            //Get customer in specific store
            await WithStore(client, async store1 =>
            {
                await WithStore(client, async store2 =>
                {
                    var stores = new List<IReferenceable<Store>>
                    {
                        store1.ToKeyResourceIdentifier(),
                        store2.ToKeyResourceIdentifier()
                    };

                    await WithCustomer(
                        client, customerDraft => DefaultCustomerDraftInStores(customerDraft, stores),
                        async customer =>
                        {
                            Assert.NotNull(customer);
                            Assert.Equal(2, customer.Stores.Count);

                            var retrievedCustomer =
                                await client
                                    .Builder()
                                    .Customers()
                                    .InStore(store1.Key)
                                    .GetById(customer.Id)
                                    .ExecuteAsync();

                            Assert.NotNull(retrievedCustomer);
                            Assert.Equal(customer.Id, retrievedCustomer.Id);
                        });
                });
            });
        }

        [Fact]
        public async Task GetCustomerByKey()
        {
            //Get Global Customer By Key
            var key = $"GetCustomerByKey-{TestingUtility.RandomString()}";
            await WithCustomer(
                client, customerDraft => DefaultCustomerDraftWithKey(customerDraft, key),
                async customer =>
                {
                    var retrievedCustomer = await client
                        .Builder()
                        .Customers()
                        .GetByKey(customer.Key)
                        .ExecuteAsync();
                    Assert.Equal(key, retrievedCustomer.Key);
                });
        }

        [Fact]
        public async Task GetCustomerInStoreByKey()
        {
            await WithStore(client, async store =>
            {
                var stores = new List<IReferenceable<Store>>
                {
                    store.ToKeyResourceIdentifier()
                };
                await WithCustomer(
                    client, customerDraft => DefaultCustomerDraftInStores(customerDraft, stores),
                    async customer =>
                    {
                        var retrievedCustomer =
                            await client
                                .Builder()
                                .Customers()
                                .GetByKey(customer.Key)
                                .InStore(store.Key)
                                .ExecuteAsync();


                        Assert.NotNull(retrievedCustomer);
                        Assert.Equal(customer.Key, retrievedCustomer.Key);
                    });
            });
        }

        [Fact]
        public async Task QueryCustomers()
        {
            var key = $"QueryCustomers-{TestingUtility.RandomString()}";
            await WithCustomer(
                client, customerDraft => DefaultCustomerDraftWithKey(customerDraft, key),
                async customer =>
                {
                    var returnedSet = await client
                        .Builder()
                        .Customers()
                        .Query()
                        .Where(c => c.Key == customer.Key.valueOf())
                        .ExecuteAsync();
                    
                    Assert.Single(returnedSet.Results);
                    Assert.Equal(key, returnedSet.Results[0].Key);
                });
        }

        [Fact]
        public async Task QueryCustomersInStore()
        {
            await WithStore(client, async store =>
            {
                var stores = new List<IReferenceable<Store>>
                {
                    store.ToKeyResourceIdentifier()
                };
                await WithCustomer(
                    client,
                    customerDraft => DefaultCustomerDraftInStores(customerDraft, stores),
                    async customer =>
                    {
                        Assert.NotNull(customer);
                        Assert.Single(customer.Stores);

                        var returnedSet = await client
                            .Builder()
                            .Customers()
                            .Query()
                            .Where(c => c.Key == customer.Key.valueOf())
                            .InStore(store.Key)
                            .ExecuteAsync();
                        
                        Assert.Single(returnedSet.Results);
                        Assert.Equal(customer.Key, returnedSet.Results[0].Key);
                    });
            });
        }

        [Fact]
        public async Task DeleteCustomerById()
        {
            var key = $"DeleteCustomerById-{TestingUtility.RandomString()}";
            await WithCustomer(
                client, customerDraft => DefaultCustomerDraftWithKey(customerDraft, key),
                async customer =>
                {
                    await client
                        .Builder()
                        .Customers()
                        .DeleteById(customer)
                        .ExecuteAsync();
                    await Assert.ThrowsAsync<NotFoundException>(
                        () => client.ExecuteAsync(new GetByIdCommand<Customer>(customer))
                    );
                });
        }

        [Fact]
        public async Task DeleteCustomerInStoreById()
        {
            await WithStore(client, async store =>
            {
                var stores = new List<IReferenceable<Store>>
                {
                    store.ToKeyResourceIdentifier()
                };
                await WithCustomer(
                    client, customerDraft => DefaultCustomerDraftInStores(customerDraft, stores),
                    async customer =>
                    {
                        Assert.NotNull(customer);
                        Assert.Single(customer.Stores);

                        await client
                            .Builder()
                            .Customers()
                            .InStore(store.Key)
                            .DeleteById(customer)
                            .ExecuteAsync();
                        await Assert.ThrowsAsync<NotFoundException>(
                            () => client.ExecuteAsync(
                                new GetByIdCommand<Customer>(customer).InStore(store.Key)));
                    });
            });
        }

        [Fact]
        public async Task DeleteCustomerByKey()
        {
            var key = $"DeleteCustomerByKey-{TestingUtility.RandomString()}";
            await WithCustomer(
                client, customerDraft => DefaultCustomerDraftWithKey(customerDraft, key),
                async customer =>
                {
                    await client
                        .Builder()
                        .Customers()
                        .DeleteByKey(customer.Key, customer.Version)
                        .ExecuteAsync();
                    await Assert.ThrowsAsync<NotFoundException>(
                        () => client.ExecuteAsync(new GetByIdCommand<Customer>(customer))
                    );
                });
        }

        [Fact]
        public async Task DeleteCustomerInStoreByKey()
        {
            await WithStore(client, async store =>
            {
                var stores = new List<IReferenceable<Store>>
                {
                    store.ToKeyResourceIdentifier()
                };
                await WithCustomer(
                    client, customerDraft => DefaultCustomerDraftInStores(customerDraft, stores),
                    async customer =>
                    {
                        Assert.NotNull(customer);
                        Assert.Single(customer.Stores);

                        await client
                            .Builder()
                            .Customers()
                            .DeleteByKey(customer.Key, customer.Version)
                            .InStore(store.Key)
                            .ExecuteAsync();
                        
                        await Assert.ThrowsAsync<NotFoundException>(
                            () => client.ExecuteAsync(
                                new GetByIdCommand<Customer>(customer).InStore(store.Key)));
                    });
            });
        }
    }
}