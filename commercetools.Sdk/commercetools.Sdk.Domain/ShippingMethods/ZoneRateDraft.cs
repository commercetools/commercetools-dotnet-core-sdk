using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.ShippingMethods
{
    public class ZoneRateDraft
    {
        [Required]
        public ResourceIdentifier Zone { get; set; }
        [Required]
        public List<ShippingRate> ShippingRates { get; set; }
    }
}