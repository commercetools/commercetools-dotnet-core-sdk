using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class RemoveParcelFromDeliveryStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "removeParcelFromDelivery";
        [Required]
        public string ParcelId { get; set; }
    }
}