using System.Linq;

namespace commercetools.Sdk.Domain
{
    [TypeMarker("giftLineItem")]
    public class GiftLineItemCartDiscountValue : CartDiscountValue
    {
        public ResourceIdentifier Product { get; set; }
        public int VariantId { get; set; }
        public ResourceIdentifier SupplyChannel { get; set; }
        public ResourceIdentifier DistributionChannel { get; set; }
    }
}