using System.Collections.Generic;
using commercetools.Sdk.Domain.Orders;


namespace commercetools.Sdk.Domain.Messages.Orders
{
    [TypeMarker("DeliveryAddressSet")]
    public class DeliveryAddressSetMessage : Message<Order>
    {
        public string DeliveryId { get; set; }
        public Address Address { get; set; }
        public Address OldAddress { get; set; }
    }
}
