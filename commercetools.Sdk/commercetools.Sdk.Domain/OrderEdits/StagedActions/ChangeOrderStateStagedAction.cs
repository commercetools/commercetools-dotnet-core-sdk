using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Orders;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class ChangeOrderStateStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "changeOrderState";
        [Required]
        public OrderState OrderState { get; set; }
    }
}