using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.ShippingMethods;

namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    public class SetShippingMethodUpdateAction : CartUpdateAction
    {
        public override string Action => "setShippingMethod";
        public IReference<ShippingMethod> ShippingMethod { get; set; }
        public ExternalTaxRateDraft ExternalTaxRate { get; set; }
    }
}