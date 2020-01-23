using commercetools.Sdk.Domain.Carts;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class SetShippingMethodTaxRateStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "setShippingMethodTaxRate";
        public ExternalTaxRateDraft ExternalTaxRate { get; set; }
    }
}