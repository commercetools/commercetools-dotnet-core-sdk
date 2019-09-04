using commercetools.Sdk.Domain.Orders;


namespace commercetools.Sdk.Domain.Messages.Orders
{
    [TypeMarker("OrderShippingAddressSet")]
    public class OrderShippingAddressSetMessage : Message<Order>
    {
        public Address Address { get; set; }
        public Address OldAddress { get; set; }
    }
}
