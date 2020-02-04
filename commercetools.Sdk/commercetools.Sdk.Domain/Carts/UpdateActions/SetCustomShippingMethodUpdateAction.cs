using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.ShippingMethods;
using commercetools.Sdk.Domain.TaxCategories;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    public class SetCustomShippingMethodUpdateAction : CartUpdateAction
    {
        public override string Action => "setCustomShippingMethod";
        [Required]
        public string ShippingMethodName { get; set; }
        public ShippingRateDraft ShippingRate { get; set; }
        public IReference<TaxCategory> TaxCategory { get; set; }
        public ExternalTaxRateDraft ExternalTaxRate { get; set; }
    }
}