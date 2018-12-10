using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class SetShippingAddressUpdateAction : UpdateAction<Order>
    {
        public string Action => "setShippingAddress";
        public Address Address { get; set; }
    }
}