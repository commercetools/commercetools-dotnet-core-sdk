using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    public class SetLineItemTaxRateUpdateAction : CartUpdateAction
    {
        public override string Action => "setLineItemTaxRate";
        [Required]
        public string LineItemId { get; set; }
        public ExternalTaxRateDraft ExternalTaxRate { get; set; }
    }
}