using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class SetShipmentReturnStateUpdateAction : UpdateAction<Order>
    {
        public string Action => "setReturnShipmentState";
        [Required]
        public string ReturnItemId { get; set; }
        [Required]
        public ReturnShipmentState ShipmentState { get; set; }
    }
}