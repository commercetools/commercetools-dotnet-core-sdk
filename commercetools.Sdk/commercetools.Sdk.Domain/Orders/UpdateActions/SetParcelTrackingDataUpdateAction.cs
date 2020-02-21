using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class SetParcelTrackingDataUpdateAction : OrderUpdateAction
    {
        public override string Action => "setParcelTrackingData";
        [Required]
        public string ParcelId { get; set; }
        public TrackingData TrackingData { get; set; }
    }
}