namespace commercetools.Sdk.Domain.Project
{
    public abstract class ShippingRateInputType
    {
        public string Type
        {
            get => this.GetType().GetTypeMarkerAttributeValue();
        }
    }
}
