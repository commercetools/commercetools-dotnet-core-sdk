using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.Orders;


namespace commercetools.Sdk.Domain.Messages.Orders
{
    [TypeMarker("OrderShippingInfoSet")]
    public class OrderShippingInfoSetMessage : Message<Order>
    {
        public ShippingInfo ShippingInfo { get; set; }
        public ShippingInfo OldShippingInfo { get; set; }
    }
}
