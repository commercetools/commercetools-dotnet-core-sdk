using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class SetDeliveryAddressUpdateAction : UpdateAction<Order>
    {
        public string Action => "setDeliveryAddress";
        [Required]
        public string DeliveryId { get; set; }
        public Address Address { get; set; }
    }
}