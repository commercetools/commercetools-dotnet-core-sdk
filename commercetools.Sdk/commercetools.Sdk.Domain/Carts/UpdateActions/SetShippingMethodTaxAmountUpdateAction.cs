namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    public class SetShippingMethodTaxAmountUpdateAction : CartUpdateAction
    {
        public override string Action => "setShippingMethodTaxAmount";
        public ExternalTaxAmountDraft ExternalTaxAmount { get; set; }
    }
}