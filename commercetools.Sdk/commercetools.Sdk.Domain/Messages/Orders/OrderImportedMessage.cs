using commercetools.Sdk.Domain.Orders;


namespace commercetools.Sdk.Domain.Messages.Orders
{
    [TypeMarker("OrderImported")]
    public class OrderImportedMessage : Message<Order>
    {
        public Order Order { get; set; }
    }
}
