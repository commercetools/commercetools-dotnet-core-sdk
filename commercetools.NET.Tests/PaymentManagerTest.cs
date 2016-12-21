using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using commercetools.Common;
using commercetools.Common.UpdateActions;
using commercetools.Customers;
using commercetools.Payments;
using commercetools.Payments.UpdateActions;
using commercetools.Messages;
using commercetools.Project;

using Newtonsoft.Json.Linq;

using NUnit.Framework;

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

            Task<Response<Project.Project>> projectTask = _client.Project().GetProjectAsync();
            projectTask.Wait();
            Assert.IsTrue(projectTask.Result.Success);
            _project = projectTask.Result.Result;

            _testCustomers = new List<Customer>();
            _testPayments = new List<Payment>();

            for (int i = 0; i < 5; i++)
            {
                CustomerDraft customerDraft = Helper.GetTestCustomerDraft();
                Task<Response<CustomerCreatedMessage>> customerCreatedTask = _client.Customers().CreateCustomerAsync(customerDraft);
                customerCreatedTask.Wait();
                Assert.IsTrue(customerCreatedTask.Result.Success);

                CustomerCreatedMessage customerCreatedMessage = customerCreatedTask.Result.Result;
                Assert.NotNull(customerCreatedMessage.Customer);
                Assert.NotNull(customerCreatedMessage.Customer.Id);

                _testCustomers.Add(customerCreatedMessage.Customer);

                PaymentDraft paymentDraft = Helper.GetTestPaymentDraft(_project, customerCreatedMessage.Customer.Id);
                Task<Response<Payment>> paymentTask = _client.Payments().CreatePaymentAsync(paymentDraft);
                paymentTask.Wait();
                Assert.IsTrue(paymentTask.Result.Success);

                Payment payment = paymentTask.Result.Result;
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
                Task task = _client.Customers().DeleteCustomerAsync(customer);
                task.Wait();
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
            Response<Payment> response = await _client.Payments().GetPaymentByIdAsync(_testPayments[0].Id);
            Assert.IsTrue(response.Success);

            Payment payment = response.Result;
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
            Response<PaymentQueryResult> response = await _client.Payments().QueryPaymentsAsync();
            Assert.IsTrue(response.Success);

            PaymentQueryResult paymentQueryResult = response.Result;
            Assert.NotNull(paymentQueryResult.Results);

            int limit = 2;
            response = await _client.Payments().QueryPaymentsAsync(limit: limit);
            Assert.IsTrue(response.Success);

            paymentQueryResult = response.Result;
            Assert.NotNull(paymentQueryResult.Results);
            Assert.LessOrEqual(paymentQueryResult.Results.Count, limit);
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
            Response<Payment> response = await _client.Payments().CreatePaymentAsync(paymentDraft);
            Assert.IsTrue(response.Success);

            Payment payment = response.Result;
            Assert.NotNull(payment.Id);

            string deletedPaymentId = payment.Id;

            Response<JObject> deleteResponse = await _client.Payments().DeletePaymentAsync(payment);
            Assert.IsTrue(deleteResponse.Success);

            response = await _client.Payments().GetPaymentByIdAsync(deletedPaymentId);
            Assert.IsFalse(response.Success);
        }

        /// <summary>
        /// Tests the PaymentManager.UpdatePaymentAsync method.
        /// </summary>
        /// <see cref="PaymentManager.UpdatePaymentAsync(commercetools.Payments.Payment, System.Collections.Generic.List{commercetools.Common.UpdateAction})"/>
        [Test]
        public async Task ShouldUpdatePaymentAsync()
        {
            Money newAmountPlanned = Helper.GetTestMoney(_project);
            string newExternalId = Helper.GetRandomNumber(10000, 99999).ToString();

            ChangeAmountPlannedAction changeAmountPlannedAction = new ChangeAmountPlannedAction(newAmountPlanned);

            GenericAction setExternalIdAction = new GenericAction("setExternalId");
            setExternalIdAction.SetProperty("externalId", newExternalId);

            List<UpdateAction> actions = new List<UpdateAction>();
            actions.Add(changeAmountPlannedAction);
            actions.Add(setExternalIdAction);

            Response<Payment> response = await _client.Payments().UpdatePaymentAsync(_testPayments[2], actions);
            Assert.IsTrue(response.Success);

            _testPayments[2] = response.Result;
            Assert.NotNull(_testPayments[2].Id);
            Assert.AreEqual(_testPayments[2].ExternalId, newExternalId);
            Assert.AreEqual(_testPayments[2].AmountPlanned.CentAmount, newAmountPlanned.CentAmount);
        }
    }
}
