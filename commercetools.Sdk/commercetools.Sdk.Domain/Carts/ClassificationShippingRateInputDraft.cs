namespace commercetools.Sdk.Domain.Carts
{
    public class ClassificationShippingRateInputDraft : ShippingRateInputDraft
    {
        // TODO Try to see if the type string can be define only once for draft and main class
        public string Type => "Classification";
        public string Key { get; set; }
    }
}