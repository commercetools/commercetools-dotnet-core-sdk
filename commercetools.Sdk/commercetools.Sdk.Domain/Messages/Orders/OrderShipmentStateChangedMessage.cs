using commercetools.Sdk.Domain.Orders;


namespace commercetools.Sdk.Domain.Messages.Orders
{
    [TypeMarker("OrderShipmentStateChanged")]
    public class OrderShipmentStateChangedMessage : Message<Order>
    {
        public ShipmentState ShipmentState { get; set; }
        public ShipmentState OldShipmentState { get; set; }
    }
}
