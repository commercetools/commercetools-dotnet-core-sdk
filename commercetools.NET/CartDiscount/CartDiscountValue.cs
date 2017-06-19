using System;
using System.Collections.Generic;
using commercetools.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace commercetools.CartDiscount
{
    public class CartDiscountValue
    {
        public CartDiscountValue(CartDiscountType type, int? perMyriad, List<Money> money, ResourceIdentifier product,
            int? variantId, ResourceIdentifier supplyChannel, ResourceIdentifier distributionChannel)
        {
            Type = type;
            PerMyriad = perMyriad;
            Money = money;
            Product = product;
            VariantId = variantId;
            SupplyChannel = supplyChannel;
            DistributionChannel = distributionChannel;
        }

        public CartDiscountValue(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            CartDiscountType discountType;
            string discountTypeStr = (data.type != null ? data.type.ToString() : string.Empty);

            this.Type = Enum.TryParse(discountTypeStr, true, out discountType) ? (CartDiscountType?)discountType : null;
            this.PerMyriad = data.permyriad;
            this.Money = Helper.GetListFromJsonArray<Money>(data.money);
            this.Product= new ResourceIdentifier(data.product);
            this.VariantId = data.variantId;
            this.SupplyChannel = new ResourceIdentifier(data.supplyChannel);
            this.DistributionChannel = new ResourceIdentifier(data.distributionChannel);
        }

        [JsonProperty(PropertyName = "type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public CartDiscountType? Type { get; private set; }

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
