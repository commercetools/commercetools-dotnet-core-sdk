using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Orders;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class ChangeShipmentStateStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "changeShipmentState";
        [Required]
        public ShipmentState ShipmentState { get; set; }
    }
}