using commercetools.Sdk.Client;
using commercetools.Sdk.Domain.Customers;
using System;
using System.Collections.Generic;

namespace commercetools.Sdk.HttpApi.IntegrationTests.Customers
{
    public class CustomerFixture : ClientFixture, IDisposable
    {
        public static readonly string Password = "1234";

        public CustomerFixture() : base()
        {
            this.CustomersToDelete = new List<Customer>();
        }

        public List<Customer> CustomersToDelete { get; private set; }

        public void Dispose()
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            this.CustomersToDelete.Reverse();
            foreach (Customer customer in this.CustomersToDelete)
            {
                Customer deletedCustomer = commerceToolsClient.ExecuteAsync(new DeleteByIdCommand<Customer>(new Guid(customer.Id), customer.Version)).Result;
            }
        }

        public CustomerDraft GetCustomerDraft()
        {
            CustomerDraft customerDraft = new CustomerDraft();
            customerDraft.Email = $"{TestingUtility.RandomString(10)}@email.com";
            customerDraft.Password = Password;
            return customerDraft;
        }

        public Customer CreateCustomer()
        {
            return this.CreateCustomer(this.GetCustomerDraft());
        }

        public Customer CreateCustomer(CustomerDraft customerDraft)
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            CustomerSignInResult customerSignInResult = commerceToolsClient.ExecuteAsync(new SignUpCustomerCommand(customerDraft)).Result as CustomerSignInResult;
            return customerSignInResult.Customer;
        }
    }
}
