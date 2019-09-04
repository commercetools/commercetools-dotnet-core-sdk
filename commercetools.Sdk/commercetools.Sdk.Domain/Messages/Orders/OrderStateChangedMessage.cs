using commercetools.Sdk.Domain.Orders;


namespace commercetools.Sdk.Domain.Messages.Orders
{
    [TypeMarker("OrderStateChanged")]
    public class OrderStateChangedMessage : Message<Order>
    {
        public OrderState OrderState { get; set; }
        public OrderState OldOrderState { get; set; }
    }
}
