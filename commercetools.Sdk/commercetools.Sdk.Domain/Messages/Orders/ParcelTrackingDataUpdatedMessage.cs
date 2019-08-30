using System.Collections.Generic;
using commercetools.Sdk.Domain.Orders;


namespace commercetools.Sdk.Domain.Messages.Orders
{
    [TypeMarker("ParcelTrackingDataUpdated")]
    public class ParcelTrackingDataUpdatedMessage : Message<Order>
    {
        public string DeliveryId { get; set; }
        public string ParcelId { get; set; }
        public TrackingData TrackingData { get; set; }
    }
}
