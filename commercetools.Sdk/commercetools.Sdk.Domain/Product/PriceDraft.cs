namespace commercetools.Sdk.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class PriceDraft
    {
        [Required]
        public BaseMoney Value { get; set; }
        [Country]
        public string Country { get; set; }
        public Reference<CustomerGroup> CustomerGroup { get; set; }
        public Reference<Channel> Channel { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidUntil { get; set; }
        public List<PriceTier> Tiers { get; set; }
        public CustomFieldsDraft Custom { get; set; }
    }
}