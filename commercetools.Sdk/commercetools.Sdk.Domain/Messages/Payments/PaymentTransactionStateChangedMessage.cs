using commercetools.Sdk.Domain.Payments;
using commercetools.Sdk.Domain.States;

namespace commercetools.Sdk.Domain.Messages.Payments
{
    [TypeMarker("PaymentTransactionStateChanged")]
    public class PaymentTransactionStateChangedMessage : Message<Payment>
    {
        public string TransactionId { get; set; }
        public TransactionState State { get; set; }
    }
}
