namespace commercetools.Sdk.Domain.Suggestions
{
    [Endpoint("product-projections/suggest")]
    public class ProductSuggestion : ISuggestion
    {
        public string Text { get; set; }
    }
}
