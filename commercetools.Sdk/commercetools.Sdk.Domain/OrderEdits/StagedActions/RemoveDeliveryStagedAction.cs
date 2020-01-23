using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class RemoveDeliveryStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "removeDelivery";
        [Required]
        public string DeliveryId { get; set; }
    }
}