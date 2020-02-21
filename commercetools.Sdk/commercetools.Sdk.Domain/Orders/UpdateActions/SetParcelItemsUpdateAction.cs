using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class SetParcelItemsUpdateAction : OrderUpdateAction
    {
        public override string Action => "setParcelItems";
        [Required]
        public string ParcelId { get; set; }
        [Required]
        public List<DeliveryItem> Items { get; set; }
    }
}