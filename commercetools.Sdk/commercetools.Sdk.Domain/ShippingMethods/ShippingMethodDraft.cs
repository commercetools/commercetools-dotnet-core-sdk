using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.TaxCategories;
using commercetools.Sdk.Linq.Discount;
using commercetools.Sdk.Registration;

namespace commercetools.Sdk.Domain.ShippingMethods
{
    public class ShippingMethodDraft : IDraft<ShippingMethod>
    {
        public string Key { get; set; }
        public LocalizedString LocalizedName { get; set; }
        public LocalizedString LocalizedDescription { get; set; }
        
        [Obsolete("Use LocalizedName instead")]
        public string Name { get; set; }
        [Obsolete("Use LocalizedDescription instead")]
        public string Description { get; set; }
        public IReference<TaxCategory> TaxCategory { get; set; }
        public List<ZoneRateDraft> ZoneRates { get; set; }
        public bool IsDefault { get; set; }
        public string Predicate { get; set; }
        public CustomFieldsDraft Custom { get; set; }
    }
}
