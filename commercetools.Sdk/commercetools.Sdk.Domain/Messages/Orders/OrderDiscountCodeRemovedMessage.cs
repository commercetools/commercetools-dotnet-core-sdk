using commercetools.Sdk.Domain.DiscountCodes;
using commercetools.Sdk.Domain.Orders;


namespace commercetools.Sdk.Domain.Messages.Orders
{
    [TypeMarker("OrderDiscountCodeRemoved")]
    public class OrderDiscountCodeRemovedMessage : Message<Order>
    {
        public Reference<DiscountCode> DiscountCode { get; set; }
    }
}
