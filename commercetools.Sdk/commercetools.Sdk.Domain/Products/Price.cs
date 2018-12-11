using System;
using System.Collections.Generic;

namespace commercetools.Sdk.Domain
{
    public class Price
    {
        public string Id { get; set; }
        public BaseMoney Value { get; set; }
        public string Country { get; set; }
        public Reference<CustomerGroup> CustomerGroup { get; set; }
        public Reference<Channel> Channel { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidUntil { get; set; }
        public List<PriceTier> Tiers { get; set; }
        public DiscountedPrice Discounted { get; set; }
        public CustomFields Custom { get; set; }
    }
}