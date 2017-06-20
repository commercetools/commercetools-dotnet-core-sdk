using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace commercetools.CartDiscounts
{
    public class CartDiscountTarget
    {
        public CartDiscountTarget(CartDiscountTargetType type, string predicate)
        {
            Type = type;
            Predicate = predicate;
        }

        public CartDiscountTarget(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            CartDiscountTargetType discountTargetType;
            string discountTargetTypeStr = (data.type != null ? data.type.ToString() : string.Empty);

            this.Type = Enum.TryParse(discountTargetTypeStr, true, out discountTargetType) ? (CartDiscountTargetType?)discountTargetType : null;
            this.Predicate = data.predicate;
        }

        [JsonProperty(PropertyName = "type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public CartDiscountTargetType? Type { get; private set; }

        /// <summary>
        /// A valid lineitem/CustomLineItem target predicate.
        /// The discount will be applied to lineitems/customlineitems that are matched by the predicate.
        /// </summary>
        [JsonProperty(PropertyName = "predicate")]
        public string Predicate { get; private set; }
    }
}
