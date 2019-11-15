using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Payments;
using Type = commercetools.Sdk.Domain.Types.Type;
using static commercetools.Sdk.IntegrationTests.GenericFixture;

namespace commercetools.Sdk.IntegrationTests.Payments
{
    public static class PaymentsFixture
    {
        #region DraftBuilds
        public static PaymentDraft DefaultPaymentDraft(PaymentDraft paymentDraft)
        {
            var random = TestingUtility.RandomInt();
            paymentDraft.Key = $"Key_{random}";
            paymentDraft.AmountPlanned = Money.FromDecimal("EUR", 1000);
            return paymentDraft;
        }
        public static PaymentDraft DefaultPaymentDraftWithKey(PaymentDraft draft, string key)
        {
            var paymentDraft = DefaultPaymentDraft(draft);
            paymentDraft.Key = key;
            return paymentDraft;
        }
        public static PaymentDraft DefaultPaymentDraftWithTransaction(PaymentDraft draft, TransactionDraft transactionDraft)
        {
            var paymentDraft = DefaultPaymentDraft(draft);
            paymentDraft.Transactions = new List<TransactionDraft> { transactionDraft };
            return paymentDraft;
        }
        public static PaymentDraft DefaultPaymentDraftWithCustomType(PaymentDraft draft, Type type, Fields fields)
        {
            var customFieldsDraft = new CustomFieldsDraft
            {
                Type = type.ToKeyResourceIdentifier(),
                Fields = fields
            };

            var paymentDraft = DefaultPaymentDraft(draft);
            paymentDraft.Custom = customFieldsDraft;

            return paymentDraft;
        }
        #endregion

        #region WithPayment

        public static async Task WithPayment( IClient client, Action<Payment> func)
        {
            await With(client, new PaymentDraft(), DefaultPaymentDraft, func);
        }
        public static async Task WithPayment( IClient client, Func<PaymentDraft, PaymentDraft> draftAction, Action<Payment> func)
        {
            await With(client, new PaymentDraft(), draftAction, func);
        }

        public static async Task WithPayment( IClient client, Func<Payment, Task> func)
        {
            await WithAsync(client, new PaymentDraft(), DefaultPaymentDraft, func);
        }
        public static async Task WithPayment( IClient client, Func<PaymentDraft, PaymentDraft> draftAction, Func<Payment, Task> func)
        {
            await WithAsync(client, new PaymentDraft(), draftAction, func);
        }
        #endregion

        #region WithUpdateablePayment

        public static async Task WithUpdateablePayment(IClient client, Func<Payment, Payment> func)
        {
            await WithUpdateable(client, new PaymentDraft(), DefaultPaymentDraft, func);
        }

        public static async Task WithUpdateablePayment(IClient client, Func<PaymentDraft, PaymentDraft> draftAction, Func<Payment, Payment> func)
        {
            await WithUpdateable(client, new PaymentDraft(), draftAction, func);
        }

        public static async Task WithUpdateablePayment(IClient client, Func<Payment, Task<Payment>> func)
        {
            await WithUpdateableAsync(client, new PaymentDraft(), DefaultPaymentDraft, func);
        }
        public static async Task WithUpdateablePayment(IClient client, Func<PaymentDraft, PaymentDraft> draftAction, Func<Payment, Task<Payment>> func)
        {
            await WithUpdateableAsync(client, new PaymentDraft(), draftAction, func);
        }

        #endregion
    }
}
