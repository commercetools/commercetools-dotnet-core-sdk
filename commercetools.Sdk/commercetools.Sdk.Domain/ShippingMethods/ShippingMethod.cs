using System;
using System.Collections.Generic;

namespace commercetools.Sdk.Domain.ShippingMethods
{
    [Endpoint("shipping-methods")]
    public class ShippingMethod
    {
        public string Id { get; set; }
        public string Key { get; set; }
        public int Version { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Reference<TaxCategory> TaxCategory { get; set; }
        public List<ZoneRateDraft> ZoneRates { get; set; }
        public bool IsDefault { get; set; }
        public string Predicate { get; set; }
    }
}