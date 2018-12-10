namespace commercetools.Sdk.Domain.ShippingMethods
{
    [TypeMarker("CartClassification")]
    public class CartClassificationShippingRatePriceTier : ShippingRatePriceTier
    {
        public string Value { get; set; }
        public Money Price { get; set; }
        public bool IsMatching { get; set; }
    }
}