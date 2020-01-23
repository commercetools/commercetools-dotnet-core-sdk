using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Orders;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class SetParcelTrackingDataStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "setParcelTrackingData";
        [Required]
        public string ParcelId { get; set; }
        public TrackingData TrackingData { get; set; }
    }
}