namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class SetLineItemTaxAmountUpdateAction : UpdateAction<Cart>
    {
        public string Action => "setLineItemTaxAmount";
        [Required]
        public string LineItemId { get; set; }
        public ExternalTaxAmountDraft ExternalTaxAmount { get; set; }
    }
}