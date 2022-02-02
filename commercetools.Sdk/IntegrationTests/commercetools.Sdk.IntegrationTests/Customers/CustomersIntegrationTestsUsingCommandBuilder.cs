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
        public async Task CreateCustomerInStore()
        {
            await WithStore(client, async store =>
            {
                var customerDraft = DefaultCustomerDraft(new CustomerDraft());

                var signInResult = (CustomerSignInResult) await client
                    .Builder()
                    .Customers()
                    .InStore(store.Key)
                    .SignUp(customerDraft)
                    .ExecuteAsync();

                var customer = signInResult.Customer;
                Assert.NotNull(customer);
                Assert.NotEmpty(customer.Stores);
                Assert.Equal(store.Key, customer.Stores[0].Key);
                await DeleteResource(client, customer);
            });
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
                            .InStore(store.Key)
                            .DeleteByKey(customer)
                            .ExecuteAsync();

                        await Assert.ThrowsAsync<NotFoundException>(
                            () => client.ExecuteAsync(
                                new GetByIdCommand<Customer>(customer).InStore(store.Key)));
                    });
            });
        }

        [Fact]
        public async Task ChangeCustomerPassword()
        {
            var oldPassword = TestingUtility.RandomString();
            await WithUpdateableCustomer(
                client,
                customerDraft => DefaultCustomerDraftWithPassword(customerDraft, oldPassword),
                async customer =>
                {
                    var newPassword = TestingUtility.RandomString();
                    var updatedCustomer = await client
                        .Builder()
                        .Customers()
                        .ChangePassword(customer, oldPassword, newPassword)
                        .ExecuteAsync();

                    //login with the new password to make Sure that password changed
                    var loginResult =
                        await client.ExecuteAsync(new LoginCustomerCommand(updatedCustomer.Email, newPassword));
                    Assert.NotNull(loginResult);
                    return updatedCustomer;
                });
        }
        
        [Fact]
        public async Task ChangeCustomerPasswordInStore()
        {
            var oldPassword = TestingUtility.RandomString();
            await WithStore(client, async store =>
            {
                var stores = new List<IReferenceable<Store>>
                {
                    store.ToKeyResourceIdentifier()
                };
                
                await WithUpdateableCustomer(
                    client,
                    customerDraft => DefaultCustomerDraftWithPassword(
                        DefaultCustomerDraftInStores(customerDraft, stores), oldPassword),
                    async customer =>
                    {
                        Assert.NotNull(customer);
                        Assert.Single(customer.Stores);
                        Assert.Equal(store.Key, customer.Stores[0].Key);
                        var newPassword = TestingUtility.RandomString();
                        var updatedCustomer = await client
                            .Builder()
                            .Customers()
                            .InStore(store.Key)
                            .ChangePassword(customer, oldPassword, newPassword)
                            .ExecuteAsync();

                        Assert.NotNull(updatedCustomer);
                        //login with the new password to make Sure that password changed
                        var loginResult = await client
                            .Builder()
                            .Customers()
                            .InStore(store.Key)
                            .Login(updatedCustomer.Email, newPassword)
                            .ExecuteAsync();
                        Assert.NotNull(loginResult);
                        return updatedCustomer;
                    });
            });
        }

        [Fact]
        public async Task ChangeCustomerPasswordInStore2()
        {
            var oldPassword = TestingUtility.RandomString();
            await WithStore(client, async store =>
            {
                var stores = new List<IReferenceable<Store>>
                {
                    store.ToKeyResourceIdentifier()
                };
                
                await WithUpdateableCustomer(
                    client,
                    customerDraft => DefaultCustomerDraftWithPassword(
                        DefaultCustomerDraftInStores(customerDraft, stores), oldPassword),
                    async customer =>
                    {
                        Assert.NotNull(customer);
                        Assert.Single(customer.Stores);
                        Assert.Equal(store.Key, customer.Stores[0].Key);
                        var newPassword = TestingUtility.RandomString();
                        var updatedCustomer = await client
                            .Builder()
                            .Customers()
                            .ChangePassword(customer, oldPassword, newPassword)
                            .InStore(store.Key)
                            .ExecuteAsync();

                        Assert.NotNull(updatedCustomer);
                        //login with the new password to make Sure that password changed
                        var loginResult = await client
                            .Builder()
                            .Customers()
                            .Login(updatedCustomer.Email, newPassword)
                            .InStore(store.Key)
                            .ExecuteAsync();
                        Assert.NotNull(loginResult);
                        return updatedCustomer;
                    });
            });
        }
        
        [Fact]
        public async Task AuthenticateCustomerInStore()
        {
            await WithStore(client, async store =>
            {
                var stores = new List<IReferenceable<Store>>
                {
                    store.ToKeyResourceIdentifier()
                };
                var password = TestingUtility.RandomString();
                await WithCustomer(
                    client,
                    draft =>
                    {
                        var customerDraft = DefaultCustomerDraftInStores(draft, stores);
                        customerDraft.Password = password;
                        return customerDraft;
                    },
                    async customer =>
                    {
                        Assert.Single(customer.Stores);
                        Assert.Equal(store.Key, customer.Stores[0].Key);
                        var result =
                            await client
                                .Builder()
                                .Customers()
                                .InStore(store.Key)
                                .Login(customer.Email, password)
                                .ExecuteAsync();
                        
                        var loginResult = result as CustomerSignInResult;
                        Assert.NotNull(loginResult);
                        Assert.NotNull(loginResult.Customer);
                        Assert.Single(loginResult.Customer.Stores);
                        Assert.Equal(store.Key, loginResult.Customer.Stores[0].Key);
                        Assert.Equal(customer.Email, loginResult.Customer.Email);
                    });
            });
        }
        
        [Fact]
        public async Task CreateTokenForResettingCustomerPasswordInStore()
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
                        
                        var result = await client
                            .Builder()
                            .Customers()
                            .InStore(store.Key)
                            .CreateTokenForPasswordResetting(customer.Email)
                            .ExecuteAsync();
                        
                        var tokenResult = result as CustomerToken;
                        Assert.NotNull(tokenResult);
                        Assert.Equal(customer.Id, tokenResult.CustomerId);
                    });
            });
        }
        
        [Fact]
        public async Task GetCustomerByPasswordTokenInStore()
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

                        var result = await client
                            .Builder()
                            .Customers()
                            .InStore(store.Key)
                            .CreateTokenForPasswordResetting(customer.Email)
                            .ExecuteAsync();
                        
                        var tokenResult = result as CustomerToken;
                        Assert.NotNull(tokenResult);
                        var retrievedCustomer =
                            await client
                                .Builder()
                                .Customers()
                                .GetByPasswordToken(tokenResult.Value)
                                .InStore(store.Key)
                                .ExecuteAsync();

                        Assert.NotNull(retrievedCustomer);
                        Assert.Single(retrievedCustomer.Stores);
                        Assert.Equal(store.Key, retrievedCustomer.Stores[0].Key);
                        Assert.Equal(customer.Email, retrievedCustomer.Email);
                    });
            });
        }
        
        [Fact]
        public async Task GetCustomerByPasswordTokenInStore2()
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

                      
                        var retrievedCustomer =
                            await client
                                .Builder()
                                .Customers()
                                .CreateTokenForPasswordResetting(customer.Email)
                                .GetByPasswordToken()
                                .InStore(store.Key)
                                .ExecuteAsync();

                        Assert.NotNull(retrievedCustomer);
                        Assert.Single(retrievedCustomer.Stores);
                        Assert.Equal(store.Key, retrievedCustomer.Stores[0].Key);
                        Assert.Equal(customer.Email, retrievedCustomer.Email);
                    });
            });
        }
        
        [Fact]
        public async Task GetCustomerByPasswordToken()
        {
            await WithCustomer(
                client, async customer =>
                {
                    Assert.NotNull(customer);

                    var retrievedCustomer =
                        await client
                            .Builder()
                            .Customers()
                            .CreateTokenForPasswordResetting(customer.Email)
                            .GetByPasswordToken()
                            .ExecuteAsync();

                    Assert.NotNull(retrievedCustomer);
                    Assert.Equal(customer.Email, retrievedCustomer.Email);
                });
        }
        

        [Fact]
        public async Task GetCustomerByEmailToken()
        {
            await WithCustomer(
                client, async customer =>
                {
                    Assert.NotNull(customer);
                  
                    var retrievedCustomer = await client
                        .Builder()
                        .Customers()
                        .CreateTokenForEmailVerification(customer.Id, 10, customer.Version)
                        .GetByEmailToken()
                        .ExecuteAsync();
                    Assert.NotNull(retrievedCustomer);
                    Assert.Equal(customer.Email, retrievedCustomer.Email);
                });
        }
        
        [Fact]
        public async Task GetCustomerByEmailTokenInStore()
        {
            await WithStore(client, async store =>
            {
                var stores = new List<IReferenceable<Store>>
                {
                    store.ToKeyResourceIdentifier()
                };
                await WithCustomer(
                    client,
                    draft => DefaultCustomerDraftInStores(draft, stores),
                    async customer =>
                    {
                        Assert.NotNull(customer);
                        Assert.Single(customer.Stores);

                        var result = await client
                            .Builder()
                            .Customers()
                            .InStore(store.Key)
                            .CreateTokenForEmailVerification(customer.Id, 10, customer.Version)
                            .ExecuteAsync();
                        
                        var tokenResult = result as CustomerToken;
                        Assert.NotNull(tokenResult);
                        

                        var retrievedCustomer = await client
                            .Builder()
                            .Customers()
                            .GetByEmailToken(tokenResult.Value)
                            .InStore(store.Key)
                            .ExecuteAsync();
                        Assert.NotNull(retrievedCustomer);
                        Assert.Equal(customer.Email, retrievedCustomer.Email);
                    });
            });
        }
        
        [Fact]
        public async Task GetCustomerByEmailTokenInStore2()
        {
            await WithStore(client, async store =>
            {
                var stores = new List<IReferenceable<Store>>
                {
                    store.ToKeyResourceIdentifier()
                };
                await WithCustomer(
                    client,
                    draft => DefaultCustomerDraftInStores(draft, stores),
                    async customer =>
                    {
                        Assert.NotNull(customer);
                        Assert.Single(customer.Stores);
                        

                        var retrievedCustomer = await client
                            .Builder()
                            .Customers()
                            .CreateTokenForEmailVerification(customer.Id, 10, customer.Version)
                            .GetByEmailToken()
                            .InStore(store.Key)
                            .ExecuteAsync();
                        Assert.NotNull(retrievedCustomer);
                        Assert.Equal(customer.Email, retrievedCustomer.Email);
                    });
            });
        }
        
        [Fact]
        public async Task ResetCustomerPasswordInStore()
        {
            await WithStore(client, async store =>
            {
                var stores = new List<IReferenceable<Store>>
                {
                    store.ToKeyResourceIdentifier()
                };
                await WithUpdateableCustomer(
                    client,
                    customerDraft => DefaultCustomerDraftInStores(customerDraft, stores),
                    async customer =>
                    {
                        var newPassword = TestingUtility.RandomString();

                        var customerWithNewPassword =
                            await client
                                .Builder()
                                .Customers()
                                .CreateTokenForPasswordResetting(customer.Email)
                                .ResetPassword(newPassword, customer.Version)
                                .InStore(store.Key)
                                .ExecuteAsync();
                        
                        Assert.NotNull(customerWithNewPassword);
                        Assert.Equal(customer.Email, customerWithNewPassword.Email);

                       
                        var signInResult =
                            await client
                                .Builder()
                                .Customers()
                                .Login(customer.Email, newPassword)
                                .InStore(store.Key)
                                .ExecuteAsync();
                        
                        var loginResult = signInResult as CustomerSignInResult;
                        Assert.NotNull(loginResult);
                        Assert.Equal(customer.Email, loginResult.Customer.Email);
                        return customerWithNewPassword;
                    });
            });
        }
        
        [Fact]
        public async Task VerifyCustomerEmailInStore()
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
                        Assert.False(customer.IsEmailVerified);
                       
                        var retrievedCustomer = await client
                            .Builder()
                            .Customers()
                            .CreateTokenForEmailVerification(customer.Id, 10, customer.Version)
                            .VerifyEmail()
                            .ExecuteAsync();
                        
                        Assert.NotNull(retrievedCustomer);
                        Assert.Equal(customer.Email, retrievedCustomer.Email);
                        Assert.True(retrievedCustomer.IsEmailVerified);
                    });
            });
        }
        
        #region UpdateActions

        [Fact]
        public async Task UpdateCustomerByKeyChangeEmailAndSetFirstName()
        {
            await WithUpdateableCustomer(client, async customer =>
            {
                var firstName = TestingUtility.RandomString();
                var newEmail = $"joe{TestingUtility.RandomString()}@example.com";
                var action1 = new ChangeEmailUpdateAction {Email = newEmail};
                var action2 = new SetFirstNameUpdateAction {FirstName = firstName};

                var updatedCustomer = await client
                    .Builder()
                    .Customers()
                    .UpdateByKey(customer)
                    .AddAction(action1)
                    .AddAction(action2)
                    .ExecuteAsync();

                Assert.Equal(firstName, updatedCustomer.FirstName);
                Assert.Equal(newEmail, updatedCustomer.Email);
                return updatedCustomer;
            });
        }

        [Fact]
        public async Task UpdateCustomerInStoreByKeySetFirstName()
        {
            await WithStore(client, async store =>
            {
                var stores = new List<IReferenceable<Store>>
                {
                    store.ToKeyResourceIdentifier()
                };
                await WithUpdateableCustomer(client,
                    draft => DefaultCustomerDraftInStores(draft, stores),
                    async customer =>
                    {
                        Assert.NotNull(customer);
                        Assert.Single(customer.Stores);

                        var firstName = TestingUtility.RandomString();
                        var action = new SetFirstNameUpdateAction {FirstName = firstName};

                        var updatedCustomer = await client
                            .Builder()
                            .Customers()
                            .UpdateByKey(customer.Key, customer.Version)
                            .AddAction(action)
                            .InStore(store.Key)
                            .ExecuteAsync();


                        Assert.Single(updatedCustomer.Stores);
                        Assert.Equal(store.Key, updatedCustomer.Stores[0].Key);
                        Assert.Equal(firstName, updatedCustomer.FirstName);
                        return updatedCustomer;
                    });
            });
        }

        [Fact]
        public async Task UpdateCustomerByIdSetLastName()
        {
            await WithUpdateableCustomer(client, async customer =>
            {
                var lastName = TestingUtility.RandomString();
                var action = new SetLastNameUpdateAction() {LastName = lastName};

                var updatedCustomer = await client
                    .Builder()
                    .Customers()
                    .UpdateById(customer)
                    .AddAction(action)
                    .ExecuteAsync();

                Assert.Equal(lastName, updatedCustomer.LastName);
                return updatedCustomer;
            });
        }

        [Fact]
        public async Task UpdateCustomerInStoreByIdSetLastName()
        {
            await WithStore(client, async store =>
            {
                var stores = new List<IReferenceable<Store>>
                {
                    store.ToKeyResourceIdentifier()
                };
                await WithUpdateableCustomer(client,
                    draft => DefaultCustomerDraftInStores(draft, stores),
                    async customer =>
                    {
                        Assert.NotNull(customer);
                        Assert.Single(customer.Stores);

                        var lastName = TestingUtility.RandomString();
                        var action = new SetLastNameUpdateAction() {LastName = lastName};

                        var updatedCustomer = await client
                            .Builder()
                            .Customers()
                            .UpdateById(customer)
                            .AddAction(action)
                            .InStore(store.Key)
                            .ExecuteAsync();

                        Assert.Single(updatedCustomer.Stores);
                        Assert.Equal(store.Key, updatedCustomer.Stores[0].Key);
                        Assert.Equal(lastName, updatedCustomer.LastName);
                        return updatedCustomer;
                    });
            });
        }

        #endregion
    }
}