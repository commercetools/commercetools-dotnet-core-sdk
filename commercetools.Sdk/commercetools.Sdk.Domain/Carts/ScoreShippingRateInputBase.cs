namespace commercetools.Sdk.Domain.Carts
{
    [TypeMarker("Score")]
    public class ScoreShippingRateInputBase
    {
        public string Type => this.GetType().GetTypeMarkerAttributeValue();
        public long Score { get; set; }
    }
}
