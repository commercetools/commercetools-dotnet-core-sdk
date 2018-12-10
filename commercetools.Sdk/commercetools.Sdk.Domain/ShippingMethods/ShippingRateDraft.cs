using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.ShippingMethods
{
    public class ShippingRateDraft
    {
        [Required]
        public Money Price { get; set; }
        public Money FreeAbove { get; set; }
        public List<ShippingRatePriceTier> Tiers { get; set; }
    }
}