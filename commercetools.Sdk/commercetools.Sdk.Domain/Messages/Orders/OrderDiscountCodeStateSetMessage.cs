using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.DiscountCodes;
using commercetools.Sdk.Domain.Orders;


namespace commercetools.Sdk.Domain.Messages.Orders
{
    [TypeMarker("OrderDiscountCodeStateSet")]
    public class OrderDiscountCodeStateSetMessage : Message<Order>
    {
        public Reference<DiscountCode> DiscountCode { get; set; }
        public DiscountCodeState State { get; set; }
        public DiscountCodeState OldState { get; set; }
    }
}
