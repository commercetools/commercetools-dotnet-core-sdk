namespace commercetools.Sdk.Domain.Projects
{
    public abstract class ShippingRateInputType
    {
        public string Type
        {
            get => this.GetType().GetTypeMarkerAttributeValue();
        }
    }
}
