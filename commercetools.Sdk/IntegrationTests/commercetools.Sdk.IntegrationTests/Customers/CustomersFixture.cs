using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Customers;
using commercetools.Sdk.Domain.Stores;
using static commercetools.Sdk.IntegrationTests.GenericFixture;
using Type = commercetools.Sdk.Domain.Types.Type;

namespace commercetools.Sdk.IntegrationTests.Customers
{
    public static class CustomersFixture
    {
        #region DraftBuilds
        public static CustomerDraft DefaultCustomerDraft(CustomerDraft customerDraft)
        {
            var rand = TestingUtility.RandomString(10);
            customerDraft.Email = $"{rand}@email.com";
            customerDraft.Password = "1234";
            customerDraft.Key = $"Key_{rand}";
            return customerDraft;
        }
        public static CustomerDraft DefaultCustomerDraftWithKey(CustomerDraft draft, string key)
        {
            var customerDraft = DefaultCustomerDraft(draft);
            customerDraft.Key = key;
            return customerDraft;
        }
        public static CustomerDraft DefaultCustomerDraftInStores(CustomerDraft draft, 
            List<ResourceIdentifier<Store>> stores)
        {
            var customerDraft = DefaultCustomerDraft(draft);
            customerDraft.Stores = stores;
            return customerDraft;
        }
        public static CustomerDraft DefaultCustomerDraftWithEmail(CustomerDraft draft, string email)
        {
            var customerDraft = DefaultCustomerDraft(draft);
            customerDraft.Email = email;
            return customerDraft;
        }
        public static CustomerDraft DefaultCustomerDraftWithPassword(CustomerDraft draft, string password)
        {
            var customerDraft = DefaultCustomerDraft(draft);
            customerDraft.Password = password;
            return customerDraft;
        }
        public static CustomerDraft DefaultCustomerDraftWithAddress(CustomerDraft draft)
        {
            var address = TestingUtility.GetRandomAddress();
            var customerDraft = DefaultCustomerDraft(draft);
            customerDraft.Addresses = new List<Address> { address };
            return customerDraft;
        }
        public static CustomerDraft DefaultCustomerDraftWithShippingAddress(CustomerDraft draft)
        {
            var address = TestingUtility.GetRandomAddress();
            var customerDraft = DefaultCustomerDraft(draft);
            customerDraft.Addresses = new List<Address> { address };
            customerDraft.ShippingAddresses = new List<int> { 0 };
            return customerDraft;
        }
        public static CustomerDraft DefaultCustomerDraftWithBillingAddress(CustomerDraft draft)
        {
            var address = TestingUtility.GetRandomAddress();
            var customerDraft = DefaultCustomerDraft(draft);
            customerDraft.Addresses = new List<Address> { address };
            customerDraft.BillingAddresses = new List<int> { 0 };
            return customerDraft;
        }
        public static CustomerDraft DefaultCustomerDraftWithCustomType(CustomerDraft draft, Type type, Fields fields)
        {
            var customFieldsDraft = new CustomFieldsDraft
            {
                Type = type.ToKeyResourceIdentifier(),
                Fields = fields
            };

            var customerDraft = DefaultCustomerDraft(draft);
            customerDraft.Custom = customFieldsDraft;

            return customerDraft;
        }
        #endregion

        public static async Task<Customer> CreateCustomer(IClient client, IDraft<Customer> buildDraft)
        {
            var signInResult = (CustomerSignInResult) await client
                .ExecuteAsync(new SignUpCustomerCommand(buildDraft));
            return signInResult.Customer;
        }

        #region WithCustomer

        public static async Task WithCustomer( IClient client, Action<Customer> func)
        {
            await With(client, new CustomerDraft(), DefaultCustomerDraft, func, CreateCustomer);
        }
        public static async Task WithCustomer( IClient client, Func<CustomerDraft, CustomerDraft> draftAction, Action<Customer> func)
        {
            await With(client, new CustomerDraft(), draftAction, func, CreateCustomer);
        }

        public static async Task WithCustomer( IClient client, Func<Customer, Task> func)
        {
            await WithAsync(client, new CustomerDraft(), DefaultCustomerDraft, func, CreateCustomer);
        }
        public static async Task WithCustomer( IClient client, Func<CustomerDraft, CustomerDraft> draftAction, Func<Customer, Task> func)
        {
            await WithAsync(client, new CustomerDraft(), draftAction, func, CreateCustomer);
        }
        #endregion

        #region WithUpdateableCustomer

        public static async Task WithUpdateableCustomer(IClient client, Func<Customer, Customer> func)
        {
            await WithUpdateable(client, new CustomerDraft(), DefaultCustomerDraft, func, CreateCustomer);
        }

        public static async Task WithUpdateableCustomer(IClient client, Func<CustomerDraft, CustomerDraft> draftAction, Func<Customer, Customer> func)
        {
            await WithUpdateable(client, new CustomerDraft(), draftAction, func, CreateCustomer);
        }

        public static async Task WithUpdateableCustomer(IClient client, Func<Customer, Task<Customer>> func)
        {
            await WithUpdateableAsync(client, new CustomerDraft(), DefaultCustomerDraft, func, CreateCustomer);
        }
        public static async Task WithUpdateableCustomer(IClient client, Func<CustomerDraft, CustomerDraft> draftAction, Func<Customer, Task<Customer>> func)
        {
            await WithUpdateableAsync(client, new CustomerDraft(), draftAction, func, CreateCustomer);
        }

        #endregion
    }
}
