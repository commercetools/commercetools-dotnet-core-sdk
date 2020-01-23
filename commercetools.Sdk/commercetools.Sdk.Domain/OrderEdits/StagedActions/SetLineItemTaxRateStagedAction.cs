using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Carts;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class SetLineItemTaxRateStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "setLineItemTaxRate";
        [Required]
        public string LineItemId { get; set; }
        public ExternalTaxRateDraft ExternalTaxRate { get; set; }
    }
}