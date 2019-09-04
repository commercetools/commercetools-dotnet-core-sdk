using commercetools.Sdk.Domain.Payments;

namespace commercetools.Sdk.Domain.Messages.Payments
{
    [TypeMarker("PaymentCreated")]
    public class PaymentCreatedMessage : Message<Payment>
    {
        public Payment Payment { get; set; }
    }
}
