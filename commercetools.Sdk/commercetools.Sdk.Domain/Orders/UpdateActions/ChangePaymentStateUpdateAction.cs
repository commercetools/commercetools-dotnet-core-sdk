using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class ChangePaymentStateUpdateAction : OrderUpdateAction
    {
        public override string Action => "changePaymentState";
        [Required]
        public PaymentState PaymentState { get; set; }
    }
}