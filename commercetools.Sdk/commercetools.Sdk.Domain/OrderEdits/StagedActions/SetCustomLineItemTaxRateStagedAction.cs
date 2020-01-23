using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Carts;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class SetCustomLineItemTaxRateStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "setCustomLineItemTaxRate";
        [Required]
        public string CustomLineItemId { get; set; }
        public ExternalTaxRateDraft ExternalTaxRate { get; set; }
    }
}