using System;
using System.Collections.Generic;
using commercetools.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace commercetools.CartDiscounts
{
    public abstract class CartDiscountValue
    {
        protected CartDiscountValue(CartDiscountType type)
        {
            Type = type;
        }

        protected CartDiscountValue(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            CartDiscountType discountType;
            string discountTypeStr = (data.type != null ? data.type.ToString() : string.Empty);
            this.Type = Enum.TryParse(discountTypeStr, true, out discountType) ? (CartDiscountType?)discountType : null;         
        }

        [JsonProperty(PropertyName = "type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public CartDiscountType? Type { get; private set; }
    }
}
