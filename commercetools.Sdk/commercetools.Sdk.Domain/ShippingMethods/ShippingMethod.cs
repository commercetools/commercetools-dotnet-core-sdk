using System;
using System.Collections.Generic;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.TaxCategories;

namespace commercetools.Sdk.Domain.ShippingMethods
{
    [Endpoint("shipping-methods")]
    [ResourceType(ReferenceTypeId.ShippingMethod)]
    public class ShippingMethod : Resource<ShippingMethod>, IKeyReferencable<ShippingMethod>
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Reference<TaxCategory> TaxCategory { get; set; }
        public List<ZoneRate> ZoneRates { get; set; }
        public bool IsDefault { get; set; }
        public string Predicate { get; set; }
    }
}
