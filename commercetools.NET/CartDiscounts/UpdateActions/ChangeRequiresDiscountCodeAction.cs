using commercetools.Common;
using Newtonsoft.Json;

namespace commercetools.CartDiscounts.UpdateActions
{
    public class ChangeRequiresDiscountCodeAction: UpdateAction
    {
        [JsonProperty(PropertyName = "requiresDiscountCode")]
        public bool RequiresDiscountCode { get; }

        public ChangeRequiresDiscountCodeAction(bool requiresDiscountCode)
        {
            this.Action = "changeRequiresDiscountCode";
            this.RequiresDiscountCode = requiresDiscountCode;
        }
    }
}
