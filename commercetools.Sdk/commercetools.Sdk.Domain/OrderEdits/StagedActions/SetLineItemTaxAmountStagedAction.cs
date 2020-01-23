using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Carts;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class SetLineItemTaxAmountStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "setLineItemTaxAmount";
        [Required]
        public string LineItemId { get; set; }
        public ExternalTaxAmountDraft ExternalTaxAmount { get; set; }
    }
}