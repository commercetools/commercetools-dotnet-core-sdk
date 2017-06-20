using commercetools.Common;
using Newtonsoft.Json;

namespace commercetools.CartDiscounts.UpdateActions
{
    public class ChangeTarget: UpdateAction
    {
        [JsonProperty(PropertyName = "target")]
        public CartDiscountTarget Target { get; }

        public ChangeTarget(CartDiscountTarget target)
        {
            this.Action = "changeTarget";
            this.Target = target;
        }
    }
}
