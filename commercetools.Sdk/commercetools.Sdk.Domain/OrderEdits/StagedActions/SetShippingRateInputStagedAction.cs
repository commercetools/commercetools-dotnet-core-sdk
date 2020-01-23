using commercetools.Sdk.Domain.Carts;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class SetShippingRateInputStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "setShippingRateInput";
        public IShippingRateInputDraft ShippingRateInput { get; set; }
    }
}