namespace commercetools.Sdk.Domain.Carts
{
    public abstract class ShippingRateInput
    {
        public string Type => this.GetType().GetTypeMarkerAttributeValue();
    }
}