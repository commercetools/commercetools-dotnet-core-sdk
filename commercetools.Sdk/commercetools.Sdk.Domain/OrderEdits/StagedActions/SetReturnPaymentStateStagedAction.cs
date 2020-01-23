using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Orders;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class SetReturnPaymentStateStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "setReturnPaymentState";
        [Required]
        public string ReturnItemId { get; set; }
        [Required]
        public ReturnPaymentState PaymentState { get; set; }
    }
}