using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.ShippingMethods;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class SetShippingMethodStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "setShippingMethod";
        public IReference<ShippingMethod> ShippingMethod { get; set; }
        public ExternalTaxRateDraft ExternalTaxRate { get; set; }
    }
}