using System.Collections.Generic;
using commercetools.Sdk.Domain.Orders;


namespace commercetools.Sdk.Domain.Messages.Orders
{
    [TypeMarker("ParcelRemovedFromDelivery")]
    public class ParcelRemovedFromDeliveryMessage : Message<Order>
    {
        public string DeliveryId { get; set; }
        public Parcel Parcel { get; set; }
    }
}
