namespace commercetools.Sdk.Domain.Carts
{
    [TypeMarker("Classification")]
    public abstract class ClassificationShippingRateInputBase
    {
        public string Type => this.GetType().GetTypeMarkerAttributeValue();
        public string Key { get; set; }
    }
}