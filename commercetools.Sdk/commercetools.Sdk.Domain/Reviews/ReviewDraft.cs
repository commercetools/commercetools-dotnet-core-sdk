using commercetools.Sdk.Domain.Customers;
using commercetools.Sdk.Domain.States;

namespace commercetools.Sdk.Domain.Reviews
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
        public IReferenceable Target { get; set; }
        public double Rating { get; set; }
        public IReferenceable<State> State { get; set; }
        public IReferenceable<Customer> Customer { get; set; }
        public CustomFields Custom { get; set; }
    }
}
