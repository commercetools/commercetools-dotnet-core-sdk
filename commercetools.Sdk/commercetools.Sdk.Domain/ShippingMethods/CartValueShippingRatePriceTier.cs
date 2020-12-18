namespace commercetools.Sdk.Domain.ShippingMethods
{
    [TypeMarker("CartValue")]
    public class CartValueShippingRatePriceTier : ShippingRatePriceTier
    {
        public long MinimumCentAmount { get; set; }
        public Money Price { get; set; }
        public bool IsMatching { get; set; }
    }
}