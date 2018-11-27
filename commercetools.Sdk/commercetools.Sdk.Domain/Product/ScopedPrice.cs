using System;

namespace commercetools.Sdk.Domain
{
    public class ScopedPrice
    {
        public string Id { get; set; }
        public Money Value { get; set; }
        public Money CurrentValue { get; set; }
        public string Country { get; set; }
        public Reference<CustomerGroup> CustomerGroup { get; set; }
        public Reference<Channel> Channel { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidUntil { get; set; }
        public DiscountedPrice Discounted { get; set; }
        public CustomFields Custom { get; set; }
    }
}