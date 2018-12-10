using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class SetCustomerIdUpdateAction : UpdateAction<Order>
    {
        public string Action => "setCustomerId";
        public string CustomerId { get; set; }
    }
}