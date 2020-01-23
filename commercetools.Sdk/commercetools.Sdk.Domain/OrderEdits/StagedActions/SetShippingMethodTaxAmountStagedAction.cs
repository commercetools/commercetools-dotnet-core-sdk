using commercetools.Sdk.Domain.Carts;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class SetShippingMethodTaxAmountStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "setShippingMethodTaxAmount";
        public ExternalTaxAmountDraft ExternalTaxAmount { get; set; }
    }
}