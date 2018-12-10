using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Payments;
using commercetools.Sdk.Domain.Validation.Attributes;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class SetDeliveryItemsUpdateAction : UpdateAction<Order>
    {
        public string Action => "setDeliveryItems";
        [Required]
        public string DeliveryId { get; set; }
        [Required]
        public List<DeliveryItem> Items { get; set; }
    }
}