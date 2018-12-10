namespace commercetools.Sdk.Domain.ShippingMethods
{
    [TypeMarker("CartValue")]
    public class CartValueShippingRatePriceTier : ShippingRatePriceTier
    {
        public int MinimumCentAmount { get; set; }
        public Money Price { get; set; }
        public bool IsMatching { get; set; }
    }
}