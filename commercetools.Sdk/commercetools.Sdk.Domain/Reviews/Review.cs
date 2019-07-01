using commercetools.Sdk.Domain.Customers;
using System;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.States;

namespace commercetools.Sdk.Domain.Reviews
{
    [Endpoint("reviews")]
    public class Review : Resource<Review>
    {
        public string Key { get; set; }
        public string UniquenessValue { get; set; }
        public string Locale { get; set; }
        public string AuthorName { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public Reference Target { get; set; }
        public int? Rating { get; set; }
        public Reference<State> State { get; set; }
        public bool IncludedInStatistics { get; set; }
        public Reference<Customer> Customer { get; set; }
        public CustomFields Custom { get; set; }
    }
}
