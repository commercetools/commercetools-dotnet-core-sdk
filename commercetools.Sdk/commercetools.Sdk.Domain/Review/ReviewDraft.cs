namespace commercetools.Sdk.Domain
{
    using commercetools.Sdk.Domain.Validation.Attributes;

    public class ReviewDraft : IDraft<Review>
    {
        public string Key { get; set; }
        public string UniquenessValue { get; set; }
        [Language]
        public string Locale { get; set; }
        public string AuthorName { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public ResourceIdentifier Target { get; set; }
        public double Rating { get; set; }
        public ResourceIdentifier State { get; set; }
        public ResourceIdentifier Customer { get; set; }
        public CustomFields Custom { get; set; }
    }
}