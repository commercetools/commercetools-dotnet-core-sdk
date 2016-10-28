using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using commercetools.Common;
using commercetools.Carts;
using commercetools.Customers;
using commercetools.Messages;
using commercetools.Project;

using Newtonsoft.Json.Linq;

using NUnit.Framework;

namespace commercetools.Tests
{
    /// <summary>
    /// Test the API methods in the CustomerManager class.
    /// </summary>
    [TestFixture]
    public class CustomerManagerTest
    {
        private Client _client;
        private Project.Project _project;
        private Cart _testCart;
        private Customer _testCustomer;

        /// <summary>
        /// Test setup
        /// </summary>
        [OneTimeSetUp]
        public void Init()
        {
            _client = new Client(Helper.GetConfiguration());

            Task<Project.Project> project = _client.Project().GetProjectAsync();
            project.Wait();
            _project = project.Result;

            CustomerDraft customerDraft = Helper.GetTestCustomerDraft();
            Task<CustomerCreatedMessage> task = _client.Customers().CreateCustomerAsync(customerDraft);
            task.Wait();

            CustomerCreatedMessage customerCreatedMessage = task.Result;

            Assert.NotNull(customerCreatedMessage.Customer);
            Assert.NotNull(customerCreatedMessage.Customer.Id);

            _testCustomer = customerCreatedMessage.Customer;

            CartDraft cartDraft = Helper.GetTestCartDraft(_project, _testCustomer.Id);
            Task<Cart> testCart = _client.Carts().CreateCartAsync(cartDraft);
            testCart.Wait();
            _testCart = testCart.Result;

            Assert.NotNull(_testCart.Id);
            Assert.AreEqual(_testCart.CustomerId, _testCustomer.Id);
        }

        /// <summary>
        /// Test teardown
        /// </summary>
        [OneTimeTearDown]
        public void Dispose()
        {
            Task<Cart> cartTask = _client.Carts().DeleteCartAsync(_testCart.Id, _testCart.Version);
            cartTask.Wait();

            Task<Customer> customerTask = _client.Customers().DeleteCustomerAsync(_testCustomer.Id, _testCustomer.Version);
            customerTask.Wait();
        }

        /// <summary>
        /// Tests the CustomerManager.GetCustomerByIdAsync method.
        /// </summary>
        /// <see cref="CustomerManager.GetCustomerByIdAsync"/>
        [Test]
        public async Task ShouldGetCustomerByIdAsync()
        {
            Customer customer = await _client.Customers().GetCustomerByIdAsync(_testCustomer.Id);

            Assert.NotNull(customer.Id);
            Assert.AreEqual(customer.Id, _testCustomer.Id);
        }

        /// <summary>
        /// Tests the CustomerManager.QueryCustomersAsync method.
        /// </summary>
        /// <see cref="CustomerManager.QueryCustomersAsync"/>
        [Test]
        public async Task ShouldQueryCustomersAsync()
        {
            CustomerQueryResult result = await _client.Customers().QueryCustomersAsync();

            Assert.NotNull(result.Results);
            Assert.GreaterOrEqual(result.Results.Count, 1);
        }

        /// <summary>
        /// Tests the CustomerManager.CreateCartAsync and CustomerManager.DeleteCartAsync methods.
        /// </summary>
        /// <see cref="CustomerManager.CreateCustomerAsync"/>
        /// <seealso cref="CustomerManager.DeleteCustomerAsync(commercetools.Customers.Customer)"/>
        [Test]
        public async Task ShouldCreateAndDeleteCustomerAsync()
        {
            CustomerDraft customerDraft = Helper.GetTestCustomerDraft();
            CustomerCreatedMessage customerCreatedMessage = await _client.Customers().CreateCustomerAsync(customerDraft);

            Assert.NotNull(customerCreatedMessage.Customer);
            Assert.NotNull(customerCreatedMessage.Customer.Id);

            Customer customer = customerCreatedMessage.Customer;

            string deletedCustomerId = customer.Id;

            customer = await _client.Customers().DeleteCustomerAsync(customer);

            AggregateException ex = Assert.Throws<AggregateException>(
                delegate
                {
                    Task<Customer> task = _client.Customers().GetCustomerByIdAsync(deletedCustomerId);
                    task.Wait();
                });
        }

        /// <summary>
        /// Tests the CustomerManager.UpdateCustomerAsync method.
        /// </summary>
        /// <see cref="CustomerManager.UpdateCustomerAsync(commercetools.Customers.Customer, System.Collections.Generic.List{Newtonsoft.Json.Linq.JObject})"/>
        [Test]
        public async Task ShouldUpdateCustomerAsync()
        {
            List<JObject> actions = new List<JObject>();

            string newEmail = string.Concat(Helper.GetRandomString(10), "@example.com");
            string newExternalId = Helper.GetRandomNumber(10000, 99999).ToString();

            actions.Add(
                JObject.FromObject(new
                {
                    action = "changeEmail",
                    email = newEmail
                })
            );

            actions.Add(
                JObject.FromObject(new
                {
                    action = "setExternalId",
                    externalId = newExternalId
                })
            );

            _testCustomer = await _client.Customers().UpdateCustomerAsync(_testCustomer, actions);

            Assert.NotNull(_testCustomer.Id);
            Assert.AreEqual(_testCustomer.Email, newEmail);
            Assert.AreEqual(_testCustomer.ExternalId, newExternalId);
        }
    }
}