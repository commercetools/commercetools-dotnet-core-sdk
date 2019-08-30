using commercetools.Sdk.Domain.Orders;


namespace commercetools.Sdk.Domain.Messages.Orders
{
    [TypeMarker("OrderReturnShipmentStateChanged")]
    public class OrderReturnShipmentStateChangedMessage : Message<Order>
    {
        public string ReturnItemId { get; set; }
        public ReturnShipmentState ReturnShipmentState { get; set; }
    }
}
