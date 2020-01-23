using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class SetLineItemCustomTypeStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "setLineItemCustomType";
        [Required]
        public string LineItemId { get; set; }
        public ResourceIdentifier Type { get; set; }
        public Fields Fields { get; set; }
    }
}