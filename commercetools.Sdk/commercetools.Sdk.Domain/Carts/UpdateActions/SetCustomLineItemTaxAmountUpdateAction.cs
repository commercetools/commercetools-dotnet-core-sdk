using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    public class SetCustomLineItemTaxAmountUpdateAction : CartUpdateAction
    {
        public override string Action => "setCustomLineItemTaxAmount";
        [Required]
        public string CustomLineItemId { get; set; }
        public ExternalTaxAmountDraft ExternalTaxAmount { get; set; }
    }
}