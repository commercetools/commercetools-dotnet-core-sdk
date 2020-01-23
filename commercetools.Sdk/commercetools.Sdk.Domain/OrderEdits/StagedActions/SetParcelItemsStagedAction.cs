using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Orders;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class SetParcelItemsStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "setParcelItems";
        [Required]
        public string ParcelId { get; set; }
        [Required]
        public List<DeliveryItem> Items { get; set; }
    }
}