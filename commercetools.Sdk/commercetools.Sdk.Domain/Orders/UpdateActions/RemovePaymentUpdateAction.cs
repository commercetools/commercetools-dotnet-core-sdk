using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Payments;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class RemovePaymentUpdateAction : UpdateAction<Order>
    {
        public string Action => "removePayment";
        [Required]
        public Reference<Payment> Payment { get; set; }
    }
}