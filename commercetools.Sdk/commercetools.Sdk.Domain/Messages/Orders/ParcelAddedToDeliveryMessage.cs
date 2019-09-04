using System.Collections.Generic;
using commercetools.Sdk.Domain.Orders;


namespace commercetools.Sdk.Domain.Messages.Orders
{
    [TypeMarker("ParcelAddedToDelivery")]
    public class ParcelAddedToDeliveryMessage : Message<Order>
    {
        public Delivery Delivery { get; set; }
        public Parcel Parcel { get; set; }
    }
}
