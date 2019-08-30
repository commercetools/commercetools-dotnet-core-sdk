using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.Orders;


namespace commercetools.Sdk.Domain.Messages.Orders
{
    [TypeMarker("OrderLineItemAdded")]
    public class OrderLineItemAddedMessage : Message<Order>
    {
        public LineItem LineItem { get; set; }
        public long AddedQuantity { get; set; }
    }
}
