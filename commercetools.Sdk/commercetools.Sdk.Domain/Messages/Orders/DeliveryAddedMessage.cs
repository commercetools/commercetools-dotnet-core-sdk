using commercetools.Sdk.Domain.Orders;


namespace commercetools.Sdk.Domain.Messages.Orders
{
    [TypeMarker("DeliveryAdded")]
    public class DeliveryAddedMessage : Message<Order>
    {
        public Delivery Delivery { get; set; }
    }
}
