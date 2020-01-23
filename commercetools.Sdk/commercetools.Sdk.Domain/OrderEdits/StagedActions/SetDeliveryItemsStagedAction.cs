using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Orders;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class SetDeliveryItemsStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "setDeliveryItems";
        [Required]
        public string DeliveryId { get; set; }
        [Required]
        public List<DeliveryItem> Items { get; set; }
    }
}