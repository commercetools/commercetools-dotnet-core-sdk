using System.Collections.Generic;
using commercetools.Sdk.Domain.Orders;


namespace commercetools.Sdk.Domain.Messages.Orders
{
    [TypeMarker("ParcelMeasurementsUpdated")]
    public class ParcelMeasurementsUpdatedMessage : Message<Order>
    {
        public string DeliveryId { get; set; }
        public string ParcelId { get; set; }
        public ParcelMeasurements Measurements { get; set; }
    }
}
