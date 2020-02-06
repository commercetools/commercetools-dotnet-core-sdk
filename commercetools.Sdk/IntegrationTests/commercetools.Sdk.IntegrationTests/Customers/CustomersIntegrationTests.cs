using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.Domain.Customers;
using commercetools.Sdk.Domain.Customers.UpdateActions;
using commercetools.Sdk.Domain.Stores;
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
    public class CustomersIntegrationTests
    {
        private readonly IClient client;

        public CustomersIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.client = serviceProviderFixture.GetService<IClient>();
        }

        [Fact]
        public async Task CreateCustomer()
        {
            var key = $"CreateCustomer-{TestingUtility.RandomString()}";
            await WithCustomer(
                client, customerDraft => DefaultCustomerDraftWithKey(customerDraft, key),
                customer => { Assert.Equal(key, customer.Key); });
        }

        [Fact]
        public async Task CreateCustomerInStore()
        {
            await WithStore(client, async store =>
            {
                var buildDraft = DefaultCustomerDraft(new CustomerDraft());
                var signInResult = (CustomerSignInResult) await client
                    .ExecuteAsync(new SignUpCustomerCommand(buildDraft).InStore(store.Key));
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
                        .ExecuteAsync(customer.ToIdResourceIdentifier().GetById());
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
                    var stores = new List<ResourceIdentifier<Store>>
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

                            //using extension method
                            var retrievedCustomer =
                                await client.ExecuteAsync(customer.ToIdResourceIdentifier().GetById()
                                    .InStore(store1.Key));

                            Assert.NotNull(retrievedCustomer);
                            Assert.Equal(customer.Id, retrievedCustomer.Id);

                            //create InStoreCommand
                            var getCustomerByIdCommand = new GetByIdCommand<Customer>(customer.Id);
                            var getCustomerInStoreByIdCommand =
                                new InStoreCommand<Customer>(store2.ToKeyResourceIdentifier(), getCustomerByIdCommand);
                            var customerInStore = await client.ExecuteAsync(getCustomerInStoreByIdCommand);
                            Assert.NotNull(customerInStore);
                            Assert.Equal(customer.Id, customerInStore.Id);
                        });
                });
            });
        }

        [Fact]
        public async Task GetCustomerByKey()
        {
            var key = $"GetCustomerByKey-{TestingUtility.RandomString()}";
            await WithCustomer(
                client, customerDraft => DefaultCustomerDraftWithKey(customerDraft, key),
                async customer =>
                {
                    var retrievedCustomer = await client
                        .ExecuteAsync(customer.ToKeyResourceIdentifier().GetByKey());
                    Assert.Equal(key, retrievedCustomer.Key);
                });
        }

        [Fact]
        public async Task GetCustomerInStoreByKey()
        {
            await WithStore(client, async store =>
            {
                var stores = new List<ResourceIdentifier<Store>>
                {
                    store.ToKeyResourceIdentifier()
                };
                await WithCustomer(
                    client, customerDraft => DefaultCustomerDraftInStores(customerDraft, stores),
                    async customer =>
                    {
                        var retrievedCustomer =
                            await client.ExecuteAsync(customer.ToKeyResourceIdentifier().GetByKey()
                                .InStore(store.Key));

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
                    var queryCommand = new QueryCommand<Customer>();
                    queryCommand.Where(p => p.Key == customer.Key.valueOf());
                    var returnedSet = await client.ExecuteAsync(queryCommand);
                    Assert.Single(returnedSet.Results);
                    Assert.Equal(key, returnedSet.Results[0].Key);
                });
        }

        [Fact]
        public async Task QueryCustomersInStore()
        {
            await WithStore(client, async store =>
            {
                var stores = new List<ResourceIdentifier<Store>>
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

                        var queryCommand = new QueryCommand<Customer>()
                            .Where(p => p.Key == customer.Key.valueOf())
                            .InStore(store.Key);
                        var returnedSet = await client.ExecuteAsync(queryCommand);
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
                    await client.ExecuteAsync(customer.DeleteById());
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
                var stores = new List<ResourceIdentifier<Store>>
                {
                    store.ToKeyResourceIdentifier()
                };
                await WithCustomer(
                    client, customerDraft => DefaultCustomerDraftInStores(customerDraft, stores),
                    async customer =>
                    {
                        Assert.NotNull(customer);
                        Assert.Single(customer.Stores);

                        await client.ExecuteAsync(customer.DeleteById().InStore(store.Key));
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
                    await client.ExecuteAsync(customer.DeleteByKey());
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
                var stores = new List<ResourceIdentifier<Store>>
                {
                    store.ToKeyResourceIdentifier()
                };
                await WithCustomer(
                    client, customerDraft => DefaultCustomerDraftInStores(customerDraft, stores),
                    async customer =>
                    {
                        Assert.NotNull(customer);
                        Assert.Single(customer.Stores);

                        await client.ExecuteAsync(customer.DeleteByKey().InStore(store.Key));
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
                client, customerDraft => DefaultCustomerDraftWithPassword(customerDraft, oldPassword), async customer =>
                {
                    var newPassword = TestingUtility.RandomString();
                    var command = new ChangeCustomerPasswordCommand(customer, oldPassword, newPassword);
                    var updatedCustomer = await client
                        .ExecuteAsync(command);
                    //login with the new password to make Sure that password changed
                    var loginResult =
                        await client.ExecuteAsync(new LoginCustomerCommand(updatedCustomer.Email, newPassword));
                    Assert.NotNull(loginResult);
                    return updatedCustomer;
                });
        }

        [Fact]
        public async Task AuthenticateAGlobalCustomer()
        {
            var password = TestingUtility.RandomString();
            await WithCustomer(
                client, customerDraft => DefaultCustomerDraftWithPassword(customerDraft, password), async customer =>
                {
                    var result =
                        await client.ExecuteAsync(new LoginCustomerCommand(customer.Email, password));
                    var loginResult = result as CustomerSignInResult;
                    Assert.NotNull(loginResult);
                    Assert.NotNull(loginResult.Customer);
                    Assert.Equal(customer.Email, loginResult.Customer.Email);
                });
        }

        [Fact]
        public async Task AuthenticateCustomerInStore()
        {
            await WithStore(client, async store =>
            {
                var stores = new List<ResourceIdentifier<Store>>
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
                            await client.ExecuteAsync(
                                new LoginCustomerCommand(customer.Email, password)
                                    .InStore(store.Key));
                        var loginResult = result as CustomerSignInResult;
                        Assert.NotNull(loginResult);
                        Assert.NotNull(loginResult.Customer);
                        Assert.Single(loginResult.Customer.Stores);
                        Assert.Equal(store.Key,loginResult.Customer.Stores[0].Key);
                        Assert.Equal(customer.Email, loginResult.Customer.Email);
                    });
            });
        }

        [Fact]
        public async Task CreateTokenForResettingPassword()
        {
            await WithCustomer(
                client, async customer =>
                {
                    var result =
                        await client.ExecuteAsync(new CreateTokenForCustomerPasswordResetCommand(customer.Email));
                    var tokenResult = result as CustomerToken;
                    Assert.NotNull(tokenResult);
                    Assert.Equal(customer.Id, tokenResult.CustomerId);
                });
        }

        [Fact]
        public async Task CreateTokenForResettingCustomerPasswordInStore()
        {
            await WithStore(client, async store =>
            {
                var stores = new List<ResourceIdentifier<Store>>
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

                        var result =
                            await client.ExecuteAsync(new CreateTokenForCustomerPasswordResetCommand(customer.Email)
                                .InStore(store.Key));
                        var tokenResult = result as CustomerToken;
                        Assert.NotNull(tokenResult);
                        Assert.Equal(customer.Id, tokenResult.CustomerId);
                    });
            });
        }

        [Fact]
        public async Task GetCustomerByPasswordToken()
        {
            await WithCustomer(
                client, async customer =>
                {
                    var result =
                        await client.ExecuteAsync(new CreateTokenForCustomerPasswordResetCommand(customer.Email));
                    var tokenResult = result as CustomerToken;
                    Assert.NotNull(tokenResult);
                    var retrievedCustomer =
                        await client.ExecuteAsync(new GetCustomerByPasswordTokenCommand(tokenResult.Value));
                    Assert.NotNull(retrievedCustomer);
                    Assert.Equal(customer.Email, retrievedCustomer.Email);
                });
        }

        [Fact]
        public async Task GetCustomerByPasswordTokenInStore()
        {
            await WithStore(client, async store =>
            {
                var stores = new List<ResourceIdentifier<Store>>
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

                        var result =
                            await client.ExecuteAsync(
                                new CreateTokenForCustomerPasswordResetCommand(customer.Email)
                                    .InStore(store.Key));
                        var tokenResult = result as CustomerToken;
                        Assert.NotNull(tokenResult);
                        var retrievedCustomer =
                            await client.ExecuteAsync(
                                new GetCustomerByPasswordTokenCommand(tokenResult.Value)
                                    .InStore(store.Key));
                        
                        Assert.NotNull(retrievedCustomer);
                        Assert.Single(retrievedCustomer.Stores);
                        Assert.Equal(store.Key,retrievedCustomer.Stores[0].Key);
                        Assert.Equal(customer.Email, retrievedCustomer.Email);
                    });
            });
        }

        [Fact]
        public async Task ResetCustomerPassword()
        {
            await WithUpdateableCustomer(
                client, async customer =>
                {
                    var result =
                        await client.ExecuteAsync(new CreateTokenForCustomerPasswordResetCommand(customer.Email));
                    var tokenResult = result as CustomerToken;
                    Assert.NotNull(tokenResult);
                    var newPassword = TestingUtility.RandomString();

                    var customerWithNewPassword =
                        await client.ExecuteAsync(new ResetCustomerPasswordCommand(tokenResult.Value, newPassword,
                            customer.Version));
                    Assert.NotNull(customerWithNewPassword);
                    Assert.Equal(customer.Email, customerWithNewPassword.Email);

                    var signInResult =
                        await client.ExecuteAsync(new LoginCustomerCommand(customer.Email, newPassword));
                    var loginResult = signInResult as CustomerSignInResult;
                    Assert.NotNull(loginResult);
                    Assert.Equal(customer.Email, loginResult.Customer.Email);
                    return customerWithNewPassword;
                });
        }

        [Fact]
        public async Task ResetCustomerPasswordInStore()
        {
            await WithStore(client, async store =>
            {
                var stores = new List<ResourceIdentifier<Store>>
                {
                    store.ToKeyResourceIdentifier()
                };
                await WithUpdateableCustomer(
                    client,
                    customerDraft => DefaultCustomerDraftInStores(customerDraft, stores),
                    async customer =>
                    {
                        var result =
                            await client.ExecuteAsync(
                                new CreateTokenForCustomerPasswordResetCommand(customer.Email)
                                    .InStore(store.Key));
                        var tokenResult = result as CustomerToken;
                        Assert.NotNull(tokenResult);
                        var newPassword = TestingUtility.RandomString();

                        var customerWithNewPassword =
                            await client.ExecuteAsync(
                                new ResetCustomerPasswordCommand(tokenResult.Value, newPassword,
                                    customer.Version).InStore(store.Key));
                        Assert.NotNull(customerWithNewPassword);
                        Assert.Equal(customer.Email, customerWithNewPassword.Email);

                        var signInResult =
                            await client.ExecuteAsync(
                                new LoginCustomerCommand(customer.Email, newPassword).InStore(store.Key));
                        var loginResult = signInResult as CustomerSignInResult;
                        Assert.NotNull(loginResult);
                        Assert.Equal(customer.Email, loginResult.Customer.Email);
                        return customerWithNewPassword;
                    });
            });
        }

        [Fact]
        public async Task VerifyCustomerEmail()
        {
            await WithCustomer(
                client, async customer =>
                {
                    Assert.False(customer.IsEmailVerified);
                    var result =
                        await client.ExecuteAsync(
                            new CreateTokenForCustomerEmailVerificationCommand(customer.Id, 10, customer.Version));
                    var tokenResult = result as CustomerToken;
                    Assert.NotNull(tokenResult);

                    var retrievedCustomer =
                        await client.ExecuteAsync(new VerifyCustomerEmailCommand(tokenResult.Value, customer.Version));
                    Assert.NotNull(retrievedCustomer);
                    Assert.Equal(customer.Email, retrievedCustomer.Email);
                    Assert.True(retrievedCustomer.IsEmailVerified);
                });
        }

        [Fact]
        public async Task VerifyCustomerEmailInStore()
        {
            await WithStore(client, async store =>
            {
                var stores = new List<ResourceIdentifier<Store>>
                {
                    store.ToKeyResourceIdentifier()
                };
                await WithCustomer(
                    client,
                    customerDraft => DefaultCustomerDraftInStores(customerDraft, stores),
                    async customer =>
                    {
                        Assert.False(customer.IsEmailVerified);
                        var result =
                            await client.ExecuteAsync(
                                new CreateTokenForCustomerEmailVerificationCommand(customer.Id, 10, customer.Version)
                                    .InStore(store.Key));
                        var tokenResult = result as CustomerToken;
                        Assert.NotNull(tokenResult);

                        var retrievedCustomer =
                            await client.ExecuteAsync(new VerifyCustomerEmailCommand(tokenResult.Value,
                                customer.Version).InStore(store.Key));
                        Assert.NotNull(retrievedCustomer);
                        Assert.Equal(customer.Email, retrievedCustomer.Email);
                        Assert.True(retrievedCustomer.IsEmailVerified);
                    });
            });
        }

        #region UpdateActions

        [Fact]
        public async Task UpdateCustomerByKeyChangeEmail()
        {
            await WithUpdateableCustomer(client, async customer =>
            {
                var newEmail = $"joe{TestingUtility.RandomString()}@example.com";
                var action = new ChangeEmailUpdateAction {Email = newEmail};

                var updatedCustomer = await client
                    .ExecuteAsync(customer.UpdateByKey(actions => actions.AddUpdate(action)));

                Assert.Equal(newEmail, updatedCustomer.Email);
                return updatedCustomer;
            });
        }

        [Fact]
        public async Task UpdateCustomerByKeySetFirstName()
        {
            await WithUpdateableCustomer(client, async customer =>
            {
                var firstName = TestingUtility.RandomString();
                var action = new SetFirstNameUpdateAction {FirstName = firstName};

                var updatedCustomer = await client
                    .ExecuteAsync(customer.UpdateByKey(actions => actions.AddUpdate(action)));

                Assert.Equal(firstName, updatedCustomer.FirstName);
                return updatedCustomer;
            });
        }

        [Fact]
        public async Task UpdateCustomerInStoreByKeySetFirstName()
        {
            await WithStore(client, async store =>
            {
                var stores = new List<ResourceIdentifier<Store>>
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
                            .ExecuteAsync(customer.UpdateByKey(
                                    actions => actions.AddUpdate(action))
                                .InStore(store.ToKeyResourceIdentifier()));

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
                    .ExecuteAsync(customer.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Equal(lastName, updatedCustomer.LastName);
                return updatedCustomer;
            });
        }

        [Fact]
        public async Task UpdateCustomerInStoreByIdSetLastName()
        {
            await WithStore(client, async store =>
            {
                var stores = new List<ResourceIdentifier<Store>>
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
                            .ExecuteAsync(customer.UpdateById(
                                    actions => actions.AddUpdate(action))
                                .InStore(store.ToKeyResourceIdentifier()));

                        Assert.Single(updatedCustomer.Stores);
                        Assert.Equal(store.Key, updatedCustomer.Stores[0].Key);
                        Assert.Equal(lastName, updatedCustomer.LastName);
                        return updatedCustomer;
                    });
            });
        }

        [Fact]
        public async Task UpdateCustomerByIdSetMiddleName()
        {
            await WithUpdateableCustomer(client, async customer =>
            {
                var middleName = TestingUtility.RandomString();
                var action = new SetMiddleNameUpdateAction {MiddleName = middleName};

                var updatedCustomer = await client
                    .ExecuteAsync(customer.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Equal(middleName, updatedCustomer.MiddleName);
                return updatedCustomer;
            });
        }

        [Fact]
        public async Task UpdateCustomerByIdSetTitle()
        {
            await WithUpdateableCustomer(client, async customer =>
            {
                var title = TestingUtility.RandomString();
                var action = new SetTitleUpdateAction {Title = title};

                var updatedCustomer = await client
                    .ExecuteAsync(customer.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Equal(title, updatedCustomer.Title);
                return updatedCustomer;
            });
        }

        [Fact]
        public async Task UpdateCustomerByIdSetSalutation()
        {
            await WithUpdateableCustomer(client, async customer =>
            {
                var salutation = TestingUtility.RandomString();
                var action = new SetSalutationUpdateAction {Salutation = salutation};

                var updatedCustomer = await client
                    .ExecuteAsync(customer.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Equal(salutation, updatedCustomer.Salutation);
                return updatedCustomer;
            });
        }

        [Fact]
        public async Task UpdateCustomerByIdAddAddress()
        {
            await WithUpdateableCustomer(client, async customer =>
            {
                Assert.Empty(customer.Addresses);
                var address = TestingUtility.GetRandomAddress();
                var action = new AddAddressUpdateAction {Address = address};

                var updatedCustomer = await client
                    .ExecuteAsync(customer.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Single(updatedCustomer.Addresses);
                Assert.Equal(address.ToString(), updatedCustomer.Addresses[0].ToString());
                return updatedCustomer;
            });
        }

        [Fact]
        public async Task UpdateCustomerChangeAddress()
        {
            await WithUpdateableCustomer(client, DefaultCustomerDraftWithAddress, async customer =>
            {
                Assert.Single(customer.Addresses);
                var address = TestingUtility.GetRandomAddress();
                var action = new ChangeAddressUpdateAction {Address = address, AddressId = customer.Addresses[0].Id};

                var updatedCustomer = await client
                    .ExecuteAsync(customer.UpdateById(actions => actions.AddUpdate(action)));


                Assert.Equal(address.ToString(), updatedCustomer.Addresses[0].ToString());
                return updatedCustomer;
            });
        }

        [Fact]
        public async Task UpdateCustomerRemoveAddress()
        {
            await WithUpdateableCustomer(client, DefaultCustomerDraftWithAddress, async customer =>
            {
                Assert.Single(customer.Addresses);
                var action = new RemoveAddressUpdateAction {AddressId = customer.Addresses[0].Id};

                var updatedCustomer = await client
                    .ExecuteAsync(customer.UpdateById(actions => actions.AddUpdate(action)));


                Assert.Empty(updatedCustomer.Addresses);
                return updatedCustomer;
            });
        }

        [Fact]
        public async Task UpdateCustomerSetDefaultShippingAddress()
        {
            await WithUpdateableCustomer(client, DefaultCustomerDraftWithAddress, async customer =>
            {
                Assert.Single(customer.Addresses);
                Assert.Null(customer.DefaultShippingAddressId);

                var shippingAddressId = customer.Addresses[0].Id;
                var action = new SetDefaultShippingAddressUpdateAction {AddressId = shippingAddressId};

                var updatedCustomer = await client
                    .ExecuteAsync(customer.UpdateById(actions => actions.AddUpdate(action)));


                Assert.Equal(shippingAddressId, updatedCustomer.DefaultShippingAddressId);
                return updatedCustomer;
            });
        }

        [Fact]
        public async Task UpdateCustomerAddShippingAddressId()
        {
            await WithUpdateableCustomer(client, DefaultCustomerDraftWithAddress, async customer =>
            {
                Assert.Single(customer.Addresses);
                Assert.Empty(customer.ShippingAddressIds);

                var shippingAddressId = customer.Addresses[0].Id;
                var action = new AddShippingAddressIdUpdateAction {AddressId = shippingAddressId};

                var updatedCustomer = await client
                    .ExecuteAsync(customer.UpdateById(actions => actions.AddUpdate(action)));


                Assert.Single(updatedCustomer.ShippingAddressIds);
                Assert.Equal(shippingAddressId, updatedCustomer.ShippingAddressIds[0]);
                return updatedCustomer;
            });
        }

        [Fact]
        public async Task UpdateCustomerRemoveShippingAddressId()
        {
            await WithUpdateableCustomer(client, DefaultCustomerDraftWithShippingAddress, async customer =>
            {
                Assert.Single(customer.ShippingAddressIds);

                var shippingAddressId = customer.ShippingAddressIds[0];
                var action = new RemoveShippingAddressIdUpdateAction {AddressId = shippingAddressId};

                var updatedCustomer = await client
                    .ExecuteAsync(customer.UpdateById(actions => actions.AddUpdate(action)));


                Assert.Empty(updatedCustomer.ShippingAddressIds);
                return updatedCustomer;
            });
        }

        [Fact]
        public async Task UpdateCustomerSetDefaultBillingAddress()
        {
            await WithUpdateableCustomer(client, DefaultCustomerDraftWithAddress, async customer =>
            {
                Assert.Single(customer.Addresses);
                Assert.Null(customer.DefaultBillingAddressId);

                var billingAddressId = customer.Addresses[0].Id;
                var action = new SetDefaultBillingAddressUpdateAction {AddressId = billingAddressId};

                var updatedCustomer = await client
                    .ExecuteAsync(customer.UpdateById(actions => actions.AddUpdate(action)));


                Assert.Equal(billingAddressId, updatedCustomer.DefaultBillingAddressId);
                return updatedCustomer;
            });
        }

        [Fact]
        public async Task UpdateCustomerAddBillingAddressId()
        {
            await WithUpdateableCustomer(client, DefaultCustomerDraftWithAddress, async customer =>
            {
                Assert.Single(customer.Addresses);
                Assert.Empty(customer.BillingAddressIds);

                var billingAddressId = customer.Addresses[0].Id;
                var action = new AddBillingAddressIdUpdateAction {AddressId = billingAddressId};

                var updatedCustomer = await client
                    .ExecuteAsync(customer.UpdateById(actions => actions.AddUpdate(action)));


                Assert.Single(updatedCustomer.BillingAddressIds);
                Assert.Equal(billingAddressId, updatedCustomer.BillingAddressIds[0]);
                return updatedCustomer;
            });
        }

        [Fact]
        public async Task UpdateCustomerRemoveBillingAddressId()
        {
            await WithUpdateableCustomer(client, DefaultCustomerDraftWithBillingAddress, async customer =>
            {
                Assert.Single(customer.BillingAddressIds);

                var billingAddressId = customer.BillingAddressIds[0];
                var action = new RemoveBillingAddressIdUpdateAction {AddressId = billingAddressId};

                var updatedCustomer = await client
                    .ExecuteAsync(customer.UpdateById(actions => actions.AddUpdate(action)));


                Assert.Empty(updatedCustomer.BillingAddressIds);
                return updatedCustomer;
            });
        }

        [Fact]
        public async Task UpdateCustomerSetCustomerGroup()
        {
            await WithCustomerGroup(client, async customerGroup =>
            {
                await WithUpdateableCustomer(client, async customer =>
                {
                    Assert.Null(customer.CustomerGroup);

                    var action = new SetCustomerGroupUpdateAction
                    {
                        CustomerGroup = customerGroup.ToKeyResourceIdentifier()
                    };

                    var updatedCustomer = await client
                        .ExecuteAsync(customer.UpdateById(actions => actions.AddUpdate(action)));


                    Assert.NotNull(updatedCustomer.CustomerGroup);
                    Assert.Equal(customerGroup.Id, updatedCustomer.CustomerGroup.Id);
                    return updatedCustomer;
                });
            });
        }

        [Fact]
        public async Task UpdateCustomerByKeySetCustomerNumber()
        {
            await WithUpdateableCustomer(client, async customer =>
            {
                var customerNumber = TestingUtility.RandomString();
                var action = new SetCustomerNumberUpdateAction {CustomerNumber = customerNumber};

                var updatedCustomer = await client
                    .ExecuteAsync(customer.UpdateByKey(actions => actions.AddUpdate(action)));

                Assert.Equal(customerNumber, updatedCustomer.CustomerNumber);
                return updatedCustomer;
            });
        }

        [Fact]
        public async Task UpdateCustomerByKeySetExternalId()
        {
            await WithUpdateableCustomer(client, async customer =>
            {
                var externalId = TestingUtility.RandomString();
                var action = new SetExternalIdUpdateAction {ExternalId = externalId};

                var updatedCustomer = await client
                    .ExecuteAsync(customer.UpdateByKey(actions => actions.AddUpdate(action)));

                Assert.Equal(externalId, updatedCustomer.ExternalId);
                return updatedCustomer;
            });
        }

        [Fact]
        public async Task UpdateCustomerSetCompanyName()
        {
            await WithUpdateableCustomer(client, async customer =>
            {
                var companyName = TestingUtility.RandomString();
                var action = new SetCompanyNameUpdateAction {CompanyName = companyName};

                var updatedCustomer = await client
                    .ExecuteAsync(customer.UpdateByKey(actions => actions.AddUpdate(action)));

                Assert.Equal(companyName, updatedCustomer.CompanyName);
                return updatedCustomer;
            });
        }

        [Fact]
        public async Task UpdateCustomerSetDateOfBirth()
        {
            await WithUpdateableCustomer(client, async customer =>
            {
                Assert.Null(customer.DateOfBirth);
                var dateOfBirth = TestingUtility.RandomDate();
                var action = new SetDateOfBirthUpdateAction {DateOfBirth = dateOfBirth};

                var updatedCustomer = await client
                    .ExecuteAsync(customer.UpdateByKey(actions => actions.AddUpdate(action)));

                Assert.NotNull(updatedCustomer.DateOfBirth);
                Assert.Equal(dateOfBirth, updatedCustomer.DateOfBirth);
                return updatedCustomer;
            });
        }

        [Fact]
        public async Task UpdateCustomerSetVatId()
        {
            await WithUpdateableCustomer(client, async customer =>
            {
                Assert.Null(customer.VatId);
                var vatId = TestingUtility.RandomString();
                var action = new SetVatIdUpdateAction {VatId = vatId};

                var updatedCustomer = await client
                    .ExecuteAsync(customer.UpdateByKey(actions => actions.AddUpdate(action)));

                Assert.Equal(vatId, updatedCustomer.VatId);
                return updatedCustomer;
            });
        }

        [Fact]
        public async Task UpdateCustomerSetCustomType()
        {
            var fields = CreateNewFields();

            await WithType(client, async type =>
            {
                await WithUpdateableCustomer(client,
                    async customer =>
                    {
                        var action = new SetCustomTypeUpdateAction
                        {
                            Type = type.ToKeyResourceIdentifier(),
                            Fields = fields
                        };

                        var updatedCustomer = await client
                            .ExecuteAsync(customer.UpdateByKey(actions => actions.AddUpdate(action)));

                        Assert.Equal(type.Id, updatedCustomer.Custom.Type.Id);
                        return updatedCustomer;
                    });
            });
        }

        [Fact]
        public async Task UpdateCustomerSetCustomField()
        {
            var fields = CreateNewFields();
            var newValue = TestingUtility.RandomString(10);

            await WithType(client, async type =>
            {
                await WithUpdateableCustomer(client,
                    customerDraft => DefaultCustomerDraftWithCustomType(customerDraft, type, fields),
                    async customer =>
                    {
                        var action = new SetCustomFieldUpdateAction()
                        {
                            Name = "string-field", Value = newValue
                        };

                        var updatedCustomer = await client
                            .ExecuteAsync(customer.UpdateByKey(actions => actions.AddUpdate(action)));

                        Assert.Equal(newValue, updatedCustomer.Custom.Fields["string-field"]);
                        return updatedCustomer;
                    });
            });
        }

        [Fact]
        public async Task UpdateCustomerSetLocale()
        {
            var projectLanguages = GetProjectLanguages(client);
            Assert.True(projectLanguages.Count > 0); //make sure that project has at least one language
            string locale = projectLanguages[0];

            await WithUpdateableCustomer(client,
                async customer =>
                {
                    Assert.Null(customer.Locale);
                    var action = new SetLocaleUpdateAction {Locale = locale};
                    var updatedCustomer = await client
                        .ExecuteAsync(customer.UpdateByKey(actions => actions.AddUpdate(action)));
                    Assert.Equal(locale, updatedCustomer.Locale);
                    return updatedCustomer;
                });
        }

        [Fact]
        public async Task UpdateCustomerByIdSetKey()
        {
            await WithUpdateableCustomer(client, async customer =>
            {
                var key = TestingUtility.RandomString();
                var action = new SetKeyUpdateAction {Key = key};

                var updatedCustomer = await client
                    .ExecuteAsync(customer.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Equal(key, updatedCustomer.Key);
                return updatedCustomer;
            });
        }

        [Fact]
        public async Task UpdateCustomerSetStores()
        {
            await WithStore(client, async store =>
            {
                var stores = new List<ResourceIdentifier<Store>>
                {
                    store.ToKeyResourceIdentifier()
                };
                await WithUpdateableCustomer(client, async customer =>
                {
                    Assert.Empty(customer.Stores);
                    var action = new SetStoresUpdateAction
                    {
                        Stores = stores
                    };

                    var updatedCustomer = await client
                        .ExecuteAsync(customer.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.Single(updatedCustomer.Stores);
                    Assert.Equal(updatedCustomer.Stores[0].Key, store.Key);
                    return updatedCustomer;
                });
            });
        }

        [Fact]
        public async Task UpdateCustomerAddStore()
        {
            await WithStore(client, async store =>
            {
                await WithUpdateableCustomer(client, async customer =>
                {
                    Assert.Empty(customer.Stores);
                    var action = new AddStoreUpdateAction
                    {
                        Store = store.ToKeyResourceIdentifier()
                    };

                    var updatedCustomer = await client
                        .ExecuteAsync(customer.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.Single(updatedCustomer.Stores);
                    Assert.Equal(updatedCustomer.Stores[0].Key, store.Key);
                    return updatedCustomer;
                });
            });
        }

        [Fact]
        public async Task UpdateCustomerRemoveStore()
        {
            await WithStore(client, async store =>
            {
                var stores = new List<ResourceIdentifier<Store>>
                {
                    store.ToKeyResourceIdentifier()
                };
                await WithUpdateableCustomer(client,
                    draft => DefaultCustomerDraftInStores(draft, stores),
                    async customer =>
                    {
                        Assert.Single(customer.Stores);
                        Assert.Equal(customer.Stores[0].Key, store.Key);
                        var action = new RemoveStoreUpdateAction
                        {
                            Store = store.ToKeyResourceIdentifier()
                        };

                        var updatedCustomer = await client
                            .ExecuteAsync(customer.UpdateById(actions => actions.AddUpdate(action)));

                        Assert.Empty(updatedCustomer.Stores);
                        return updatedCustomer;
                    });
            });
        }

        #endregion
    }
}