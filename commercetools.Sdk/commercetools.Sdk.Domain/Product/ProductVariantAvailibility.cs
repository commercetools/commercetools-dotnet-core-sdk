using System.Collections.Generic;

namespace commercetools.Sdk.Domain
{
    public class ProductVariantAvailability
    {
        public bool IsOnStock { get; set;  }
        public int RestockableInDays { get; set; }
        public int AvailableQuantity { get; set; }
        public Dictionary<string, ProductVariantAvailability> Channels { get; set; }
    }

    // TODO See if t his class should be used instead in channels
    public class Availability
    {
        public bool IsOnStock { get; set; }
        public int RestockableInDays { get; set; }
        public int AvailableQuantity { get; set; }
    }
}