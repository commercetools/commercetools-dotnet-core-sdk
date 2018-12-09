namespace commercetools.Sdk.Domain.Carts
{
    [TypeMarker("Score")]
    public class ScoreShippingRateInput : ShippingRateInput
    {
        public double Score { get; set; }
    }
}