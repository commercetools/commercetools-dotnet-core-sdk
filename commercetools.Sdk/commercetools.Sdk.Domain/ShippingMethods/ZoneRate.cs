using System.Collections.Generic;
using commercetools.Sdk.Domain.Zones;

namespace commercetools.Sdk.Domain.ShippingMethods
{
    public class ZoneRate
    {
        public Reference<Zone> Zone { get; set; }
        public List<ShippingRate> ShippingRates { get; set; }
    }
}