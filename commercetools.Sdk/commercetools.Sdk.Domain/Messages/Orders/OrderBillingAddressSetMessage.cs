using commercetools.Sdk.Domain.Orders;


namespace commercetools.Sdk.Domain.Messages.Orders
{
    [TypeMarker("OrderBillingAddressSet")]
    public class OrderBillingAddressSetMessage : Message<Order>
    {
        public Address Address { get; set; }
        public Address OldAddress { get; set; }
    }
}
