using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class ChangeShipmentStateUpdateAction : OrderUpdateAction
    {
        public override string Action => "changeShipmentState";
        [Required]
        public ShipmentState ShipmentState { get; set; }
    }
}