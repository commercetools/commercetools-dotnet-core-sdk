using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class SetLineItemPriceStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "setLineItemPrice";
        [Required]
        public string LineItemId { get; set; }
        public BaseMoney ExternalPrice { get; set; }
    }
}