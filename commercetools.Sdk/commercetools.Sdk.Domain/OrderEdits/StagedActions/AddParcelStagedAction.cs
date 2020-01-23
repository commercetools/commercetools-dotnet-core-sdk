using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Orders;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class AddParcelStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "addParcelToDelivery";
        [Required]
        public string DeliveryId { get; set; }
        public ParcelMeasurements Measurements { get; set; }
        public TrackingData TrackingData { get; set; }
        public List<DeliveryItem> Items { get; set; }
    }
}