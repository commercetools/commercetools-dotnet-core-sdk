using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.Orders;


namespace commercetools.Sdk.Domain.Messages.Orders
{
    [TypeMarker("OrderCustomLineItemQuantityChanged")]
    public class OrderCustomLineItemQuantityChangedMessage : Message<Order>
    {
        public string CustomLineItemId { get; set; }
        public long Quantity { get; set; }
        public long OldQuantity { get; set; }
    }
}
