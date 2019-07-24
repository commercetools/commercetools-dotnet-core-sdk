using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Zones;

namespace commercetools.Sdk.Domain.ShippingMethods
{
    public class ZoneRateDraft
    {
        [Required]
        public IReference<Zone> Zone { get; set; }
        [Required]
        public List<ShippingRateDraft> ShippingRates { get; set; }
    }
}
