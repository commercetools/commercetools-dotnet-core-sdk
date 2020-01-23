using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Carts;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class SetCustomLineItemTaxAmountStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "setCustomLineItemTaxAmount";
        [Required]
        public string CustomLineItemId { get; set; }
        public ExternalTaxAmountDraft ExternalTaxAmount { get; set; }
    }
}