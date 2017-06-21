using commercetools.Common;
using Newtonsoft.Json;

namespace commercetools.CartDiscounts.UpdateActions
{
    public class ChangeValueAction : UpdateAction
    {
        [JsonProperty(PropertyName = "value")]
        public CartDiscountValue Value { get; }

        public ChangeValueAction(CartDiscountValue value)
        {
            this.Action = "changeValue";
            this.Value = value;
        }
    }
}
