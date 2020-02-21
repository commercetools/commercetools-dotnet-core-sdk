using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Payments;
using commercetools.Sdk.Domain.Validation.Attributes;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class RemoveParcelFromDeliveryUpdateAction : OrderUpdateAction
    {
        public override string Action => "removeParcelFromDelivery";
        [Required]
        public string ParcelId { get; set; }
    }
}