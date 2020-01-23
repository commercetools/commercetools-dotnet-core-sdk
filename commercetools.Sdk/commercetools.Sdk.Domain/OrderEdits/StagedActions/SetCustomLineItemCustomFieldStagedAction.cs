using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class SetCustomLineItemCustomFieldStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "setCustomLineItemCustomField";
        [Required]
        public string CustomLineItemId { get; set; }
        [Required]
        public string Name { get; set; }
        public object Value { get; set; }
    }
}