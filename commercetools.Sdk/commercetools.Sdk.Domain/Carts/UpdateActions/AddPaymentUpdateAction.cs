using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Payments;

namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class AddPaymentUpdateAction : UpdateAction<Cart>
    {
        public string Action => "addPayment";
        [Required]
        public IReference<Payment> Payment { get; set; }
    }
}