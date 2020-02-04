using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class SetParcelMeasurementsUpdateAction : OrderUpdateAction
    {
        public override string Action => "setParcelMeasurements";
        [Required]
        public string ParcelId { get; set; }
        public ParcelMeasurements Measurements { get; set; }
    }
}
