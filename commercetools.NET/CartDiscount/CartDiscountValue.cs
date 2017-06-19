using System.Collections.Generic;
using commercetools.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace commercetools.CartDiscount
{
    public class CartDiscountValue
    {
        [JsonProperty(PropertyName = "type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public CartDiscountType Type { get; private set; }

        [JsonProperty(PropertyName = "permyriad")]
        public int? PerMyriad { get; private set; }

        [JsonProperty(PropertyName = "money")]
        public List<Money> Money { get; private set; }

        [JsonProperty(PropertyName = "product")]
        public ResourceIdentifier Product { get; private set; }

        [JsonProperty(PropertyName = "variantId")]
        public int? VariantId { get; private set; }

        [JsonProperty(PropertyName = "supplyChannel")]
        public ResourceIdentifier SupplyChannel { get; private set; }

        [JsonProperty(PropertyName = "distributionChannel")]
        public ResourceIdentifier DistributionChannel { get; private set; }
    }
}
