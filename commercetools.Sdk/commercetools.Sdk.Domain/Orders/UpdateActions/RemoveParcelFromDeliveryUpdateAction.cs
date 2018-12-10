using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Payments;
using commercetools.Sdk.Domain.Validation.Attributes;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class RemoveParcelFromDeliveryUpdateAction : UpdateAction<Order>
    {
        public string Action => "removeParcelFromDelivery";
        [Required]
        public string ParcelId { get; set; }
    }
}