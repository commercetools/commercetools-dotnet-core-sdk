namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class SetCustomLineItemTaxRateUpdateAction : UpdateAction<Cart>
    {
        public string Action => "setCustomLineItemTaxRate";
        [Required]
        public string CustomLineItemId { get; set; }
        public ExternalTaxRateDraft ExternalTaxRate { get; set; }
    }
}