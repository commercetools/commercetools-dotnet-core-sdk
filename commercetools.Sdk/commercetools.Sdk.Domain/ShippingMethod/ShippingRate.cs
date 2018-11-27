using System.Collections.Generic;

namespace commercetools.Sdk.Domain
{
    public class ShippingRate
    {
        public CentPrecisionMoney Price { get; set; }
        public CentPrecisionMoney FreeAbove { get; set; }
        public List<ShippingRatePriceTier> Tiers { get; set; }
        public bool IsMatching { get; set; }
    }
}