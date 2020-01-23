using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Orders;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class ChangePaymentStateStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "changeOrderState";
        [Required]
        public PaymentState PaymentState { get; set; }
    }
}