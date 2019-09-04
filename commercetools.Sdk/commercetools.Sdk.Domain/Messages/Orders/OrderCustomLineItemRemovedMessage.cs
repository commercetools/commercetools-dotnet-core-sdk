using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.Orders;


namespace commercetools.Sdk.Domain.Messages.Orders
{
    [TypeMarker("OrderCustomLineItemRemoved")]
    public class OrderCustomLineItemRemovedMessage : Message<Order>
    {
        public string CustomLineItemId { get; set; }
        public CustomLineItem CustomLineItem { get; set; }
    }
}
