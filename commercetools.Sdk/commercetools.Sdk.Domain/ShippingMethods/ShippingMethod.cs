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
        public LocalizedString LocalizedName { get; set; }
        public LocalizedString LocalizedDescription { get; set; }
        
        [Obsolete("Use LocalizedName instead")]
        public string Name { get; set; }
        [Obsolete("Use LocalizedDescription instead")]
        public string Description { get; set; }
        public Reference<TaxCategory> TaxCategory { get; set; }
        public List<ZoneRate> ZoneRates { get; set; }
        public bool IsDefault { get; set; }
        public string Predicate { get; set; }
        public CustomFields Custom { get; set; }
    }
}