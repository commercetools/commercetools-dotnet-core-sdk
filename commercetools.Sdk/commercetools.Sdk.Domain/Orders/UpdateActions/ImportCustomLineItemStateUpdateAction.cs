using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class ImportCustomLineItemStateUpdateAction : OrderUpdateAction
    {
        public override string Action => "importCustomLineItemState";
        [Required]
        public string CustomLineItemId { get; set; }
        [Required]
        public List<ItemState> State { get; set; }
    }
}