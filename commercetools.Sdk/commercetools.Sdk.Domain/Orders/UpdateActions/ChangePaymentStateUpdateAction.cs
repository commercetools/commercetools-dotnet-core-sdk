using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class ChangePaymentStateUpdateAction : UpdateAction<Order>
    {
        public string Action => "changePaymentState";
        [Required]
        public PaymentState PaymentState { get; set; }
    }
}