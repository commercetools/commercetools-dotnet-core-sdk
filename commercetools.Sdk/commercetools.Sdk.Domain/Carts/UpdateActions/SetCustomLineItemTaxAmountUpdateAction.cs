namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class SetCustomLineItemTaxAmountUpdateAction : UpdateAction<Cart>
    {
        public string Action => "setCustomLineItemTaxAmount";
        [Required]
        public string CustomLineItemId { get; set; }
        public ExternalTaxAmountDraft ExternalTaxAmount { get; set; }
    }
}