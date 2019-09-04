using commercetools.Sdk.Domain.Payments;
using commercetools.Sdk.Domain.States;

namespace commercetools.Sdk.Domain.Messages.Payments
{
    [TypeMarker("PaymentCreated")]
    public class PaymentStatusStateTransitionMessage : Message<Payment>
    {
        public Reference<State> State { get; set; }
        public bool Force { get; set; }
    }
}
