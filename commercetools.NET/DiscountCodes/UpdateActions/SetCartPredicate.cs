using commercetools.Common;
using Newtonsoft.Json;

namespace commercetools.DiscountCodes.UpdateActions
{
    public class SetCartPredicate : UpdateAction
    {
        [JsonProperty(PropertyName = "cartPredicate")]
        public string CartPredicate { get; set; }

        public SetCartPredicate()
        {
            this.Action = "setCartPredicate";
        }
    }
}
