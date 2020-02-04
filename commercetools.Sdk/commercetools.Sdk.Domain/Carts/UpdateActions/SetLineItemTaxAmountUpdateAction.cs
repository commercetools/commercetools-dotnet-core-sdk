using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    public class SetLineItemTaxAmountUpdateAction : CartUpdateAction
    {
        public override string Action => "setLineItemTaxAmount";
        [Required]
        public string LineItemId { get; set; }
        public ExternalTaxAmountDraft ExternalTaxAmount { get; set; }
    }
}