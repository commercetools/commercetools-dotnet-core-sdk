using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Orders;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class ImportCustomLineItemStateStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "importCustomLineItemState";
        [Required]
        public string CustomLineItemId { get; set; }
        [Required]
        public List<ItemState> State { get; set; }
    }
}