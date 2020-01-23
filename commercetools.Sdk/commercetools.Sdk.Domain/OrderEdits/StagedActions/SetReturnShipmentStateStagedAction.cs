using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Orders;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class SetReturnShipmentStateStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "setReturnShipmentState";
        [Required]
        public string ReturnItemId { get; set; }
        [Required]
        public ReturnShipmentState ShipmentState { get; set; }
    }
}