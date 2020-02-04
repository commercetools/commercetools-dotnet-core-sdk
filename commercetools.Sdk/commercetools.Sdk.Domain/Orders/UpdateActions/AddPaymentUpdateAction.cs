using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Payments;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class AddPaymentUpdateAction : OrderUpdateAction
    {
        public override string Action => "addPayment";
        [Required]
        public IReference<Payment> Payment { get; set; }
    }
}