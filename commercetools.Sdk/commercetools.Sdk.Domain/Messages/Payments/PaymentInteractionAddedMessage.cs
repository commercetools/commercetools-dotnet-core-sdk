using commercetools.Sdk.Domain.Payments;

namespace commercetools.Sdk.Domain.Messages.Payments
{
    [TypeMarker("PaymentInteractionAdded")]
    public class PaymentInteractionAddedMessage : Message<Payment>
    {
        public CustomFields Interaction { get; set; }
    }
}
