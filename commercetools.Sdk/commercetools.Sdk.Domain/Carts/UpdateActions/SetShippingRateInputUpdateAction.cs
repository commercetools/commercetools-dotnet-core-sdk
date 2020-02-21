namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    public class SetShippingRateInputUpdateAction : CartUpdateAction
    {
        public override string Action => "setShippingRateInput";
        public IShippingRateInputDraft ShippingRateInput { get; set; }
    }
}