using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.ShippingMethods;
using commercetools.Sdk.Domain.TaxCategories;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class SetCustomShippingMethodStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "setCustomShippingMethod";
        [Required]
        public string ShippingMethodName { get; set; }
        public ShippingRateDraft ShippingRate { get; set; }
        public IReference<TaxCategory> TaxCategory { get; set; }
        public ExternalTaxRateDraft ExternalTaxRate { get; set; }
    }
}