using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class RemoveCustomLineItemStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "removeCustomLineItem";
        [Required]
        public string CustomLineItemId { get; set; }
    }
}