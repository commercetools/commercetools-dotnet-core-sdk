using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class ChangeOrderStateUpdateAction : UpdateAction<Order>
    {
        public string Action => "changeOrderState";
        [Required]
        public OrderState OrderState { get; set; }
    }
}