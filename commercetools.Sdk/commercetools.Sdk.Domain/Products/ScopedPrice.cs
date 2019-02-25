using commercetools.Sdk.Domain.CustomerGroups;
using System;
using commercetools.Sdk.Domain.Channels;

namespace commercetools.Sdk.Domain
{
    public class ScopedPrice
    {
        public string Id { get; set; }
        public BaseMoney Value { get; set; }
        public BaseMoney CurrentValue { get; set; }
        public string Country { get; set; }
        public Reference<CustomerGroup> CustomerGroup { get; set; }
        public Reference<Channel> Channel { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidUntil { get; set; }
        public DiscountedPrice Discounted { get; set; }
        public CustomFields Custom { get; set; }
    }
}