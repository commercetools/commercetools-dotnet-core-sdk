using commercetools.Common;
using Newtonsoft.Json;

namespace commercetools.CartDiscounts.UpdateActions
{
    public class ChangeValue : UpdateAction
    {
        [JsonProperty(PropertyName = "value")]
        public CartDiscountValue Value { get; }

        public ChangeValue(CartDiscountValue value)
        {
            this.Action = "changeValue";
            this.Value = value;
        }
    }
}
