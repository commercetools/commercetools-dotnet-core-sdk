namespace commercetools.Sdk.Domain
{
    [Endpoint("reviews")]
    public class Review
    {
        public string Id { get; set; }

        // Can be either a product or a channel
        public Reference Target { get; set; }
    }
}