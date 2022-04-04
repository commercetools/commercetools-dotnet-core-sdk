using System;
using System.Collections.Generic;

namespace commercetools.Sdk.Domain.Orders
{
    public class ParcelDraft
    {
        public ParcelMeasurements Measurements { get; set; }
        public TrackingData TrackingData { get; set; }
        public List<DeliveryItem> Items { get; set; }
        public CustomFields Custom { get; set; }
    }
}