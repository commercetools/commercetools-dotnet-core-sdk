using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Orders;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class SetCustomLineItemCustomTypeStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "setCustomLineItemCustomType";
        [Required]
        public string CustomLineItemId { get; set; }
        public ResourceIdentifier Type { get; set; }
        public Fields Fields { get; set; }
    }
}