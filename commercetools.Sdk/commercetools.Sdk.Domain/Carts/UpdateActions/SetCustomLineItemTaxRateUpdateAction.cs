using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    public class SetCustomLineItemTaxRateUpdateAction : CartUpdateAction
    {
        public override string Action => "setCustomLineItemTaxRate";
        [Required]
        public string CustomLineItemId { get; set; }
        public ExternalTaxRateDraft ExternalTaxRate { get; set; }
    }
}