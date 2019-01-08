using commercetools.Sdk.Domain.Customers;
using System;

namespace commercetools.Sdk.Domain
{
    [Endpoint("reviews")]
    public class Review
    {
        public string Id { get; set; }
        public int Version { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public string Key { get; set; }
        public string UniquenessValue { get; set; }
        public string Locale { get; set; }
        public string AuthorName { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public Reference Target { get; set; }
        public double Rating { get; set; }
        public Reference<State> State { get; set; }
        public bool IncludedInStatistics { get; set; }
        public Reference<Customer> Customer { get; set; }
        public CustomFields Custom { get; set; }
    }
}