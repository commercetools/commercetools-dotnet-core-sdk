using commercetools.Sdk.Domain.Orders;


namespace commercetools.Sdk.Domain.Messages.Orders
{
    [TypeMarker("OrderPaymentStateChanged")]
    public class OrderPaymentStateChangedMessage : Message<Order>
    {
        public PaymentState PaymentState { get; set; }
        public PaymentState OldPaymentState { get; set; }
    }
}
