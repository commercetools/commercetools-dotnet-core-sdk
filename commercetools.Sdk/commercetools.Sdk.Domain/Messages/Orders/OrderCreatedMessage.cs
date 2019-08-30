using commercetools.Sdk.Domain.Orders;


namespace commercetools.Sdk.Domain.Messages.Orders
{
    [TypeMarker("OrderCreated")]
    public class OrderCreatedMessage : Message<Order>
    {
        public Order Order { get; set; }
    }
}
