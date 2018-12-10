using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Payments;
using commercetools.Sdk.Domain.Validation.Attributes;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class SetParcelItemsUpdateAction : UpdateAction<Order>
    {
        public string Action => "setParcelItems";
        [Required]
        public string ParcelId { get; set; }
        [Required]
        public List<DeliveryItem> Items { get; set; }
    }
}