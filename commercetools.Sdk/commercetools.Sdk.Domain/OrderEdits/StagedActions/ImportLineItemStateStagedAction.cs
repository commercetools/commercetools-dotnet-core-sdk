using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Orders;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class ImportLineItemStateStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "importLineItemState";
        [Required]
        public string LineItemId { get; set; }
        [Required]
        public List<ItemState> State { get; set; }
    }
}