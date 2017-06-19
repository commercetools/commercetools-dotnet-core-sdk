using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace commercetools.CartDiscount
{
    public class CartDiscountTarget
    {
        [JsonProperty(PropertyName = "type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public CartDiscountTargetType Type { get; private set; }

        /// <summary>
        /// A valid lineitem/CustomLineItem target predicate.
        /// The discount will be applied to lineitems/customlineitems that are matched by the predicate.
        /// </summary>
        [JsonProperty(PropertyName = "predicate")]
        public string Predicate { get; private set; }
    }
}
