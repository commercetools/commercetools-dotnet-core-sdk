using commercetools.Sdk.Domain.Payments;

namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class AddPaymentUpdateAction : UpdateAction<Cart>
    {
        public string Action => "addPayment";
        [Required]
        public Reference<Payment> Payment { get; set; }
    }
}