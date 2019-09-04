using commercetools.Sdk.Domain.DiscountCodes;
using commercetools.Sdk.Domain.Orders;


namespace commercetools.Sdk.Domain.Messages.Orders
{
    [TypeMarker("OrderDiscountCodeAdded")]
    public class OrderDiscountCodeAddedMessage : Message<Order>
    {
        public Reference<DiscountCode> DiscountCode { get; set; }
    }
}
