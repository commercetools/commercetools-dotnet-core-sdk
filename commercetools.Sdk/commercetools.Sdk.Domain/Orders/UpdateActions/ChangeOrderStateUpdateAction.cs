using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class ChangeOrderStateUpdateAction : OrderUpdateAction
    {
        public override string Action => "changeOrderState";
        [Required]
        public OrderState OrderState { get; set; }
    }
}