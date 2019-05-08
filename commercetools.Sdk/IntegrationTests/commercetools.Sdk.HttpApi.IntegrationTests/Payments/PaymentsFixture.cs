using System;
using System.Collections.Generic;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Customers;
using commercetools.Sdk.Domain.Payments;
using commercetools.Sdk.Domain.ShoppingLists;
using commercetools.Sdk.HttpApi.IntegrationTests.Customers;

namespace commercetools.Sdk.HttpApi.IntegrationTests.Payments
{
    public class PaymentsFixture : ClientFixture, IDisposable
    {
        public List<Payment> PaymentsToDelete { get; private set; }

        public PaymentsFixture() : base()
        {
            this.PaymentsToDelete = new List<Payment>();
        }

        public void Dispose()
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            this.PaymentsToDelete.Reverse();
            foreach (Payment payment in this.PaymentsToDelete)
            {
                Payment deletedType = commerceToolsClient.ExecuteAsync(new DeleteByIdCommand<Payment>(new Guid(payment.Id), payment.Version)).Result;
            }
        }

        /// <summary>
        /// Get Payment Draft
        /// </summary>
        /// <returns></returns>

        public PaymentDraft GetPaymentDraft()
        {
            int centAmount = TestingUtility.RandomInt(100, 10000);
            PaymentDraft paymentDraft = new PaymentDraft
            {
                Key = TestingUtility.RandomString(10),
                AmountPlanned = Money.Parse($"{centAmount} EUR")
            };
            return paymentDraft;
        }

        public Payment CreatePayment()
        {
            return this.CreatePayment(this.GetPaymentDraft());
        }

        public Payment CreatePayment(PaymentDraft paymentDraft)
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            Payment payment = commerceToolsClient.ExecuteAsync(new CreateCommand<Payment>(paymentDraft)).Result;
            return payment;
        }

    }
}
