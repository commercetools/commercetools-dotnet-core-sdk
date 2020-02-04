using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class SetReturnPaymentStateUpdateAction : OrderUpdateAction
    {
        public override string Action => "setReturnPaymentState";
        [Required]
        public string ReturnItemId { get; set; }
        [Required]
        public ReturnPaymentState PaymentState { get; set; }
    }
}