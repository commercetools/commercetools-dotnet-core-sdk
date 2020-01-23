using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Orders;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class SetParcelMeasurementsStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "setParcelMeasurements";
        [Required]
        public string ParcelId { get; set; }
        public ParcelMeasurements Measurements { get; set; }
    }
}
