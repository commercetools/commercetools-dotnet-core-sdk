using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Payments;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class RemovePaymentUpdateAction : OrderUpdateAction
    {
        public override string Action => "removePayment";
        [Required]
        public IReference<Payment> Payment { get; set; }
    }
}