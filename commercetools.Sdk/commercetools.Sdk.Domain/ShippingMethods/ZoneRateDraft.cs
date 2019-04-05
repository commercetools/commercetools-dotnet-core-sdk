using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Zones;

namespace commercetools.Sdk.Domain.ShippingMethods
{
    public class ZoneRateDraft
    {
        [Required]
        public IReferenceable<Zone> Zone { get; set; }
        [Required]
        public List<ShippingRate> ShippingRates { get; set; }
    }
}
