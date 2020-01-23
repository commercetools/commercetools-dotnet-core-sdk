using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class SetLineItemCustomFieldStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "setLineItemCustomField";
        [Required]
        public string LineItemId { get; set; }
        [Required]
        public string Name { get; set; }
        public object Value { get; set; }
    }
}