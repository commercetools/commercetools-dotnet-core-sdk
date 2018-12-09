namespace commercetools.Sdk.Domain.Carts
{
    public class ScoreShippingRateInputDraft : ShippingRateInputDraft
    {
        // TODO Try to see if the type string can be define only once for draft and main class
        public string Type => "Score";
        public double Score { get; set; }
    }
}