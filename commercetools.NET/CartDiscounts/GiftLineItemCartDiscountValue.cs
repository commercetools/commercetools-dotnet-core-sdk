using System;
using commercetools.Common;
using Newtonsoft.Json;

namespace commercetools.CartDiscounts
{
    public class GiftLineItemCartDiscountValue : CartDiscountValue
    {
        [JsonProperty(PropertyName = "product")]
        public ResourceIdentifier Product { get; private set; }

        [JsonProperty(PropertyName = "variantId")]
        public int VariantId { get; private set; }

        [JsonProperty(PropertyName = "supplyChannel")]
        public ResourceIdentifier SupplyChannel { get; set; }

        [JsonProperty(PropertyName = "distributionChannel")]
        public ResourceIdentifier DistributionChannel { get; set; }

        public GiftLineItemCartDiscountValue(
            ResourceIdentifier product,
            int variantId) : base(CartDiscountType.GiftLineItem)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            Product = product;
            VariantId = variantId;
        }

        public GiftLineItemCartDiscountValue(dynamic data) : base((object)data)
        {
            this.Product = new ResourceIdentifier(data.product);
            this.VariantId = data.variantId;
            this.SupplyChannel = new ResourceIdentifier(data.supplyChannel);
            this.DistributionChannel = new ResourceIdentifier(data.distributionChannel);
        }
    }
}
