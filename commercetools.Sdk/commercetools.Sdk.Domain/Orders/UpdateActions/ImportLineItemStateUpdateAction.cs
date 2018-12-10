using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class ImportLineItemStateUpdateAction : UpdateAction<Order>
    {
        public string Action => "importLineItemState";
        [Required]
        public string LineItemId { get; set; }
        [Required]
        public List<ItemState> State { get; set; }
    }
}