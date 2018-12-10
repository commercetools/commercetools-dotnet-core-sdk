namespace commercetools.Sdk.Domain.ShippingMethods
{
    public abstract class ShippingRatePriceTier
    {
        public string Type => this.GetType().GetTypeMarkerAttributeValue();
    }
}