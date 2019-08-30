using commercetools.Sdk.Domain.Orders;


namespace commercetools.Sdk.Domain.Messages.Orders
{
    [TypeMarker("DeliveryRemoved")]
    public class DeliveryRemovedMessage : Message<Order>
    {
        public Delivery Delivery { get; set; }
    }
}
