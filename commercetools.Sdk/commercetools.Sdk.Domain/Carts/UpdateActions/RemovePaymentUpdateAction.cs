using commercetools.Sdk.Domain.Payments;

namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class RemovePaymentUpdateAction : UpdateAction<Cart>
    {
        public string Action => "removePayment";
        [Required]
        public Reference<Payment> Payment { get; set; }
    }
}