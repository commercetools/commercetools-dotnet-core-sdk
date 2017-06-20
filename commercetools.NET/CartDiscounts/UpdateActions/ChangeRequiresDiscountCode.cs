using commercetools.Common;
using Newtonsoft.Json;

namespace commercetools.CartDiscounts.UpdateActions
{
    public class ChangeRequiresDiscountCode: UpdateAction
    {
        [JsonProperty(PropertyName = "requiresDiscountCode")]
        public bool RequiresDiscountCode { get; }

        public ChangeRequiresDiscountCode(bool requiresDiscountCode)
        {
            this.Action = "changeRequiresDiscountCode";
            this.RequiresDiscountCode = requiresDiscountCode;
        }
    }
}
