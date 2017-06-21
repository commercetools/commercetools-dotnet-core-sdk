using commercetools.Common;
using Newtonsoft.Json;

namespace commercetools.CartDiscounts.UpdateActions
{
    public class ChangeCartPredicateAction: UpdateAction
    {
        [JsonProperty(PropertyName = "cartPredicate")]
        public string CartPredicate { get; }

        public ChangeCartPredicateAction(string cartPredicate)
        {
            this.Action = "changeCartPredicate";
            this.CartPredicate = cartPredicate;
        }
    }
}
