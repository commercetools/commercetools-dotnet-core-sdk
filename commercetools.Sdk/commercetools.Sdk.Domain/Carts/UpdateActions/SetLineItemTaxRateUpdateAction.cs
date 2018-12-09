namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class SetLineItemTaxRateUpdateAction : UpdateAction<Cart>
    {
        public string Action => "setLineItemTaxRate";
        [Required]
        public string LineItemId { get; set; }
        public ExternalTaxRateDraft ExternalTaxRate { get; set; }
    }
}