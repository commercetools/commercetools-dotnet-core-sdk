using commercetools.Common;
using Newtonsoft.Json;

namespace commercetools.CartDiscounts.UpdateActions
{
    public class ChangeTargetAction: UpdateAction
    {
        [JsonProperty(PropertyName = "target")]
        public CartDiscountTarget Target { get; }

        public ChangeTargetAction(CartDiscountTarget target)
        {
            this.Action = "changeTarget";
            this.Target = target;
        }
    }
}
