using System.Collections.Generic;

namespace commercetools.Sdk.Domain.ShippingMethods
{
    public class ShippingRate
    {
        public Money Price { get; set; }
        public Money FreeAbove { get; set; }
        public List<ShippingRatePriceTier> Tiers { get; set; }
        public bool IsMatching { get; set; }
    }
}