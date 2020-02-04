using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class SetReturnShipmentStateUpdateAction : OrderUpdateAction
    {
        public override string Action => "setReturnShipmentState";
        [Required]
        public string ReturnItemId { get; set; }
        [Required]
        public ReturnShipmentState ShipmentState { get; set; }
    }
}