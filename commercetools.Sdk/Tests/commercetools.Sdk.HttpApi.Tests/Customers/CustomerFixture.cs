using commercetools.Sdk.Client;
using commercetools.Sdk.Domain.Customers;
using System;
using System.Collections.Generic;

namespace commercetools.Sdk.HttpApi.Tests.Customers
{
    public class CustomerFixture : ClientFixture, IDisposable
    {
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
            customerDraft.Email = "email@email.com";
            customerDraft.Password = "1234";
            return customerDraft;
        }
    }
}
