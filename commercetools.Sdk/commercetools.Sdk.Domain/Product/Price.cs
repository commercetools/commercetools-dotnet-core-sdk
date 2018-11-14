using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Domain
{
    public class Price
    {
        public string Id { get; set; }
        public Money Value { get; set; }
        // TODO Add validation
        public string Country { get; set; }
        public Reference CustomerGroup { get; set; }
        public Reference<Channel> Channel { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidUntil { get; set; }
        public List<PriceTier> Tiers { get; set; }
        public DiscountedPrice Discounted { get; set; }
        public CustomFields Custom { get; set; }
    }
}
