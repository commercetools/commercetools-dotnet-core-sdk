using commercetools.Sdk.Domain.ShippingMethods;
using commercetools.Sdk.Domain.TaxCategories;

namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using commercetools.Sdk.Domain.Validation.Attributes;

    public class SetCustomShippingMethodUpdateAction : UpdateAction<Cart>
    {
        public string Action => "setCustomShippingMethod";
        [Required]
        public string ShippingMethodName { get; set; }
        public ShippingRateDraft ShippingRate { get; set; }
        public Reference<TaxCategory> TaxCategory { get; set; }
        public ExternalTaxRateDraft ExternalTaxRate { get; set; }
    }
}