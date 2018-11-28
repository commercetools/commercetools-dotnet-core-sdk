using System.Collections.Generic;

namespace commercetools.Sdk.Domain
{
    public class ProductVariantAvailability : ProductVariantAvailabilityBase
    {
        public Dictionary<string, ProductVariantAvailabilityBase> Channels { get; set; }
    }   
}