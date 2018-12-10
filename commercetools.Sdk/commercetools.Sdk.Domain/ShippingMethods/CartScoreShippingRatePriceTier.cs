namespace commercetools.Sdk.Domain.ShippingMethods
{
    [TypeMarker("CartScore")]
    public class CartScoreShippingRatePriceTier : ShippingRatePriceTier
    {
        // TODO See if fixed price and function can be combined in one class
        public double Score { get; set; }
        public Money Price { get; set; }
        public bool IsMatching { get; set; }

        public PriceFunction PriceFunction { get; set; }

    }
}