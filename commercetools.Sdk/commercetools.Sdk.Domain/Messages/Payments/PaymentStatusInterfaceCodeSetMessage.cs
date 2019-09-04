using commercetools.Sdk.Domain.Payments;

namespace commercetools.Sdk.Domain.Messages.Payments
{
    [TypeMarker("PaymentStatusInterfaceCodeSet")]
    public class PaymentStatusInterfaceCodeSetMessage : Message<Payment>
    {
        public string InterfaceCode { get; set; }
    }
}
