using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Payments;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class AddPaymentUpdateAction : UpdateAction<Order>
    {
        public string Action => "addPayment";
        [Required]
        public IReference<Payment> Payment { get; set; }
    }
}