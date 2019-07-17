using commercetools.Sdk.Domain.Channels;
using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain.CartDiscounts
{
    [TypeMarker("giftLineItem")]
    public class GiftLineItemCartDiscountValue : CartDiscountValue
    {
        public IReference<Product> Product { get; set; }
        public int VariantId { get; set; }
        public IReference<Channel> SupplyChannel { get; set; }
        public IReference<Channel> DistributionChannel { get; set; }
    }
}
