using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.Orders;


namespace commercetools.Sdk.Domain.Messages.Orders
{
    [TypeMarker("OrderShippingRateInputSet")]
    public class OrderShippingRateInputSetMessage : Message<Order>
    {
        public IShippingRateInput ShippingRateInput { get; set; }
        public IShippingRateInput OldShippingRateInput { get; set; }
    }
}
