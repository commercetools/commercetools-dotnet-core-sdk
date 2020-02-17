namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    public class SetShippingMethodTaxRateUpdateAction : CartUpdateAction
    {
        public override string Action => "setShippingMethodTaxRate";
        public ExternalTaxRateDraft ExternalTaxRate { get; set; }
    }
}