using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.Orders;


namespace commercetools.Sdk.Domain.Messages.Orders
{
    [TypeMarker("OrderCustomLineItemAdded")]
    public class OrderCustomLineItemAddedMessage : Message<Order>
    {
        public CustomLineItem CustomLineItem { get; set; }
    }
}
