using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class ChangeShipmentStateUpdateAction : UpdateAction<Order>
    {
        public string Action => "changeShipmentState";
        [Required]
        public ShipmentState ShipmentState { get; set; }
    }
}