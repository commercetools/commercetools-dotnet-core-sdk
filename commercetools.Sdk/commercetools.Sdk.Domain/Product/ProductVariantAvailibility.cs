using System.Collections.Generic;

namespace commercetools.Sdk.Domain
{
    public class ProductVariantAvailabilityChannel : ProductVariantAvailability
    {
        public Dictionary<string, ProductVariantAvailability> Channels { get; set; }
    }

    public class ProductVariantAvailability
    {
        public bool IsOnStock { get; set; }
        public int RestockableInDays { get; set; }
        public int AvailableQuantity { get; set; }
    }
}