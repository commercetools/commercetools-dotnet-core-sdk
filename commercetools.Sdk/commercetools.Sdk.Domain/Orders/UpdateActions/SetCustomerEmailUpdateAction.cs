using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class SetCustomerEmailUpdateAction : UpdateAction<Order>
    {
        public string Action => "setCustomerEmail";
        public string Email { get; set; }
    }
}