using System.Collections.Generic;

namespace commercetools.Sdk.Domain
{
    public class ProductVariantAvailibility
    {
        public bool IsOnStock { get; set;  }
        public int RestockableInDays { get; set; }
        public int AvailableQuantity { get; set; }
        public Dictionary<string, ProductVariantAvailibility> Channels { get; set; }
    }

    // TODO See if t his class should be used instead in channels
    public class Availability
    {
        public bool IsOnStock { get; set; }
        public int RestockableInDays { get; set; }
        public int AvailableQuantity { get; set; }
    }
}