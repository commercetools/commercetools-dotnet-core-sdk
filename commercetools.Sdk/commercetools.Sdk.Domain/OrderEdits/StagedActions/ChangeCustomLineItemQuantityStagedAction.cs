using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class ChangeCustomLineItemQuantityStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "changeCustomLineItemQuantity";
        [Required]
        public string CustomLineItemId { get; set; }
        [Required]
        public long Quantity { get; set; }
    }
}
