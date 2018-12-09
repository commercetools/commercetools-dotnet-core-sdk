namespace commercetools.Sdk.Domain.Carts
{
    [TypeMarker("Classification")]
    public class ClassificationShippingRateInput : ShippingRateInput
    {
        public string Key { get; set; }
        public LocalizedString Label { get; set; }
    }
}