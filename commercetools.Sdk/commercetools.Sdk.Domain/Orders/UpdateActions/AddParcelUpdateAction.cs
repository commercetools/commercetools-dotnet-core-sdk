using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class AddParcelUpdateAction : UpdateAction<Order>
    {
        public string Action => "addParcelToDelivery";
        [Required]
        public string DeliveryId { get; set; }
        public ParcelMeasurements Measurements { get; set; }
        public TrackingData TrackingData { get; set; }
        public List<DeliveryItem> Items { get; set; }
    }
}