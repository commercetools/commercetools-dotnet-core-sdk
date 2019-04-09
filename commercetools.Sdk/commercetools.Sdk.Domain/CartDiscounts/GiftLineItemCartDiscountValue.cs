using System.Linq;
using commercetools.Sdk.Domain.Channels;

namespace commercetools.Sdk.Domain
{
    [TypeMarker("giftLineItem")]
    public class GiftLineItemCartDiscountValue : CartDiscountValue
    {
        public IReferenceable<Product> Product { get; set; }
        public int VariantId { get; set; }
        public IReferenceable<Channel> SupplyChannel { get; set; }
        public IReferenceable<Channel> DistributionChannel { get; set; }
    }
}
