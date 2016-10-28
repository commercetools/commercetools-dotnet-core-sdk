using System.Collections.Generic;
using System.Threading.Tasks;

using commercetools.Common;
using commercetools.Carts;
using commercetools.Customers;
using commercetools.Payments;
using commercetools.Messages;
using commercetools.Project;

using Newtonsoft.Json.Linq;

using NUnit.Framework;
using System;

namespace commercetools.Tests
{
    /// <summary>
    /// Test the API methods in the PaymentManager class.
    /// </summary>
    [TestFixture]
    public class PaymentManagerTest
    {
        private Client _client;
        private Project.Project _project;
        private List<Customer> _testCustomers;
        private List<Payment> _testPayments;

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

            _testCustomers = new List<Customer>();
            _testPayments = new List<Payment>();

            for (int i = 0; i < 5; i++)
            {
                CustomerDraft customerDraft = Helper.GetTestCustomerDraft();
                Task<CustomerCreatedMessage> customerTask = _client.Customers().CreateCustomerAsync(customerDraft);
                customerTask.Wait();
                CustomerCreatedMessage customerCreatedMessage = customerTask.Result;

                Assert.NotNull(customerCreatedMessage.Customer);
                Assert.NotNull(customerCreatedMessage.Customer.Id);

                _testCustomers.Add(customerCreatedMessage.Customer);

                PaymentDraft paymentDraft = Helper.GetTestPaymentDraft(_project, customerCreatedMessage.Customer.Id);
                Task<Payment> paymentTask = _client.Payments().CreatePaymentAsync(paymentDraft);
                paymentTask.Wait();
                Payment payment = paymentTask.Result;

                Assert.NotNull(payment.Id);

                _testPayments.Add(payment);
            }
             
        }

        /// <summary>
        /// Test teardown
        /// </summary>
        [OneTimeTearDown]
        public void Dispose()
        {
            foreach (Customer customer in _testCustomers)
            {
                Task<Customer> customerTask = _client.Customers().DeleteCustomerAsync(customer);
                customerTask.Wait();
            }

            foreach (Payment payment in _testPayments)
            {
                Task task = _client.Payments().DeletePaymentAsync(payment.Id, payment.Version);
                task.Wait();
            }
        }

        /// <summary>
        /// Tests the PaymentManager.GetPaymentByIdAsync method.
        /// </summary>
        /// <see cref="PaymentManager.GetPaymentByIdAsync"/>
        [Test]
        public async Task ShouldGetPaymentByIdAsync()
        {
            Payment payment = await _client.Payments().GetPaymentByIdAsync(_testPayments[0].Id);

            Assert.NotNull(payment.Id);
            Assert.AreEqual(payment.Id, _testPayments[0].Id);
        }

        /// <summary>
        /// Tests the PaymentManager.QueryPaymentsAsync method.
        /// </summary>
        /// <see cref="PaymentManager.QueryPaymentsAsync"/>
        [Test]
        public async Task ShouldQueryPaymentsAsync()
        {
            PaymentQueryResult result = await _client.Payments().QueryPaymentsAsync();

            Assert.NotNull(result);
            Assert.NotNull(result.Results);

            int limit = 2;
            result = await _client.Payments().QueryPaymentsAsync(limit: limit);

            Assert.NotNull(result);
            Assert.NotNull(result.Results);
            Assert.LessOrEqual(result.Results.Count, limit);
        }

        /// <summary>
        /// Tests the PaymentManager.CreateCartAsync and PaymentManager.DeleteCartAsync methods.
        /// </summary>
        /// <see cref="PaymentManager.CreatePaymentAsync"/>
        /// <seealso cref="PaymentManager.DeletePaymentAsync(commercetools.Payments.Payment)"/>
        [Test]
        public async Task ShouldCreateAndDeletePaymentAsync()
        {
            PaymentDraft paymentDraft = Helper.GetTestPaymentDraft(_project, _testCustomers[1].Id);
            Payment payment = await _client.Payments().CreatePaymentAsync(paymentDraft);

            Assert.NotNull(payment.Id);

            string deletedPaymentId = payment.Id;

            await _client.Payments().DeletePaymentAsync(payment);

            AggregateException ex = Assert.Throws<AggregateException>(
                delegate
                {
                    Task task = _client.Payments().GetPaymentByIdAsync(deletedPaymentId);
                    task.Wait();
                });
        }

        /// <summary>
        /// Tests the PaymentManager.UpdatePaymentAsync method.
        /// </summary>
        /// <see cref="PaymentManager.UpdatePaymentAsync(commercetools.Payments.Payment, System.Collections.Generic.List{Newtonsoft.Json.Linq.JObject})"/>
        [Test]
        public async Task ShouldUpdatePaymentAsync()
        {
            List<JObject> actions = new List<JObject>();

            Money newAmountPlanned = Helper.GetTestMoney(_project);
            string newExternalId = Helper.GetRandomNumber(10000, 99999).ToString();

            actions.Add(
                JObject.FromObject(new
                {
                    action = "changeAmountPlanned",
                    amount = newAmountPlanned
                })
            );

            actions.Add(
                JObject.FromObject(new
                {
                    action = "setExternalId",
                    externalId = newExternalId
                })
            );

            _testPayments[2] = await _client.Payments().UpdatePaymentAsync(_testPayments[2], actions);

            Assert.NotNull(_testPayments[2].Id);
            Assert.AreEqual(_testPayments[2].ExternalId, newExternalId);
            Assert.AreEqual(_testPayments[2].AmountPlanned.CentAmount, newAmountPlanned.CentAmount);
        }
    }
}