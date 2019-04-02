namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class AddCustomLineItemUpdateAction : UpdateAction<Cart>
    {
        public string Action => "addCustomLineItem";
        [Required]
        public LocalizedString Name { get; set; }
        public long Quantity { get; set; }
        [Required]
        public BaseMoney Money { get; set; }
        [Required]
        public string Slug { get; set; }
        [Required]
        public Reference<TaxCategory> TaxCategory { get; set; }
        public ExternalTaxRateDraft ExternalTaxRate { get; set; }
        public CustomFieldsDraft Custom { get; set; }
        public ItemShippingDetailsDraft ShippingDetails { get; set; }
    }
}
