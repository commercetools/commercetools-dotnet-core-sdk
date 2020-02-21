using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class SetDeliveryItemsUpdateAction : OrderUpdateAction
    {
        public override string Action => "setDeliveryItems";
        [Required]
        public string DeliveryId { get; set; }
        [Required]
        public List<DeliveryItem> Items { get; set; }
    }
}