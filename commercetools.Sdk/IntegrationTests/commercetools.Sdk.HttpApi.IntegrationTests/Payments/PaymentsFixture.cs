using System;
using System.Collections.Generic;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Customers;
using commercetools.Sdk.Domain.Payments;
using commercetools.Sdk.Domain.ShoppingLists;
using commercetools.Sdk.Domain.States;
using commercetools.Sdk.HttpApi.IntegrationTests.Customers;
using commercetools.Sdk.HttpApi.IntegrationTests.States;
using Type = commercetools.Sdk.Domain.Type;
using Xunit.Abstractions;

namespace commercetools.Sdk.HttpApi.IntegrationTests.Payments
{
    public class PaymentsFixture : ClientFixture, IDisposable
    {
        public List<Payment> PaymentsToDelete { get; private set; }

        private readonly CustomerFixture customerFixture;
        private readonly StatesFixture statesFixture;
        private readonly TypeFixture typeFixture;


        public PaymentsFixture(ServiceProviderFixture serviceProviderFixture) : base(serviceProviderFixture)
        {
            this.PaymentsToDelete = new List<Payment>();
            this.customerFixture = new CustomerFixture(serviceProviderFixture);
            this.typeFixture = new TypeFixture(serviceProviderFixture);
            this.statesFixture = new StatesFixture(serviceProviderFixture);
        }

        public void Dispose()
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            this.PaymentsToDelete.Reverse();
            foreach (Payment payment in this.PaymentsToDelete)
            {
                Payment deletedType = commerceToolsClient.ExecuteAsync(new DeleteByIdCommand<Payment>(new Guid(payment.Id), payment.Version)).Result;
            }
            this.customerFixture.Dispose();
            this.typeFixture.Dispose();
            this.statesFixture.Dispose();
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
                AmountPlanned = Money.Parse($"{centAmount} EUR"),
                Transactions = new List<TransactionDraft>()
            };
            return paymentDraft;
        }

        public PaymentDraft GetPaymentDraftWithCustomFields()
        {
            var customFieldsDraft = CreateCustomFieldsDraft();
            int centAmount = TestingUtility.RandomInt(100, 10000);
            PaymentDraft paymentDraft = new PaymentDraft
            {
                Key = TestingUtility.RandomString(10),
                AmountPlanned = Money.Parse($"{centAmount} EUR"),
                Custom = customFieldsDraft,
                Transactions = new List<TransactionDraft>()
            };
            return paymentDraft;
        }

        private CustomFieldsDraft CreateCustomFieldsDraft()
        {
            CustomFieldsDraft customFieldsDraft = new CustomFieldsDraft();
            Type type = this.typeFixture.CreateType();
            this.typeFixture.TypesToDelete.Add(type);
            customFieldsDraft.Type = new ResourceIdentifier<Type> {Key = type.Key};
            customFieldsDraft.Fields = this.CreateNewFields();
            return customFieldsDraft;
        }

        public Payment CreatePayment()
        {
            return this.CreatePayment(this.GetPaymentDraft());
        }
        public Payment CreatePaymentWithTransaction()
        {
            var refundTransaction = this.GetTransactionDraft(DateTime.Now, TransactionType.Refund,
                Money.Parse($"{TestingUtility.RandomInt(100, 10000)} EUR"), TestingUtility.RandomString(10), TransactionState.Initial);
            var paymentDraft = this.GetPaymentDraft();
            paymentDraft.Transactions.Add(refundTransaction);
            return this.CreatePayment(paymentDraft);
        }
        public Payment CreatePaymentWithCustomFields()
        {
            return this.CreatePayment(this.GetPaymentDraftWithCustomFields());
        }

        public Payment CreatePayment(PaymentDraft paymentDraft)
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            Payment payment = commerceToolsClient.ExecuteAsync(new CreateCommand<Payment>(paymentDraft)).Result;
            return payment;
        }

        public Customer CreateCustomer()
        {
            Customer customer = this.customerFixture.CreateCustomer();
            this.customerFixture.CustomersToDelete.Add(customer);
            return customer;
        }
        public Fields CreateNewFields()
        {
            Fields fields = this.typeFixture.CreateNewFields();
            return fields;
        }

        public Type CreateNewType()
        {
            Type type = this.typeFixture.CreateType();
            this.typeFixture.TypesToDelete.Add(type);
            return type;
        }
        public State CreateNewState(StateType stateType = StateType.ProductState,bool initial = true)
        {
            State state = this.statesFixture.CreateState(stateType, initial);
            this.statesFixture.StatesToDelete.Add(state);
            return state;
        }

        public TransactionDraft GetTransactionDraft(DateTime timestamp, TransactionType type, Money amount, string interactionId, TransactionState state)
        {
            var transactionDraft = new TransactionDraft
            {
                Timestamp = timestamp,
                State = state,
                Amount = amount,
                InteractionId = interactionId,
                Type = type
            };
            return transactionDraft;
        }
    }
}
