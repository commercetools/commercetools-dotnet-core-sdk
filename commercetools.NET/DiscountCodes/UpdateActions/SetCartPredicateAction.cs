using commercetools.Common;
using Newtonsoft.Json;

namespace commercetools.DiscountCodes.UpdateActions
{
    public class SetCartPredicateAction : UpdateAction
    {
        [JsonProperty(PropertyName = "cartPredicate")]
        public string CartPredicate { get; set; }

        public SetCartPredicateAction()
        {
            this.Action = "setCartPredicate";
        }
    }
}
