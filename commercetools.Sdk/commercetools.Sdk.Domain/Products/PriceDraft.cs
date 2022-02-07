using commercetools.Sdk.Domain.Channels;
using commercetools.Sdk.Domain.Validation.Attributes;

namespace commercetools.Sdk.Domain
{
    using commercetools.Sdk.Domain.CustomerGroups;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class PriceDraft
    {
        [Required]
        public BaseMoney Value { get; set; }
        [Country]
        public string Country { get; set; }
        public ResourceIdentifier<CustomerGroup> CustomerGroup { get; set; }
        public ResourceIdentifier<Channel> Channel { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidUntil { get; set; }
        public List<PriceTier> Tiers { get; set; }
        public CustomFieldsDraft Custom { get; set; }
    }
}
