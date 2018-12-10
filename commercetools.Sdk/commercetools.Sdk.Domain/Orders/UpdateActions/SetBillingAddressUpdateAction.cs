using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class SetBillingAddressUpdateAction : UpdateAction<Order>
    {
        public string Action => "setBillingAddress";
        public Address Address { get; set; }
    }
}