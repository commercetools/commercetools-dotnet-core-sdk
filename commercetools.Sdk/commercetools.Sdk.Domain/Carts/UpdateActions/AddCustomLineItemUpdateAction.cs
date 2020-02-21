using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.TaxCategories;

namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    public class AddCustomLineItemUpdateAction : CartUpdateAction
    {
        public override string Action => "addCustomLineItem";
        [Required]
        public LocalizedString Name { get; set; }
        public long Quantity { get; set; }
        [Required]
        public BaseMoney Money { get; set; }
        [Required]
        public string Slug { get; set; }
        [Required]
        public IReference<TaxCategory> TaxCategory { get; set; }
        public ExternalTaxRateDraft ExternalTaxRate { get; set; }
        public CustomFieldsDraft Custom { get; set; }
        public ItemShippingDetailsDraft ShippingDetails { get; set; }
    }
}
