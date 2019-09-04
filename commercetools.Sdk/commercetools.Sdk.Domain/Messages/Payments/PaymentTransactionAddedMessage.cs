using commercetools.Sdk.Domain.Payments;

namespace commercetools.Sdk.Domain.Messages.Payments
{
    [TypeMarker("PaymentTransactionAdded")]
    public class PaymentTransactionAddedMessage : Message<Payment>
    {
        public Transaction Transaction { get; set; }
    }
}
