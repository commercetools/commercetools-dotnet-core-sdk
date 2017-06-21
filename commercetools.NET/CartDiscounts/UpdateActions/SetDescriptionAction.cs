using commercetools.Common;
using Newtonsoft.Json;

namespace commercetools.CartDiscounts.UpdateActions
{
    public class SetDescriptionAction: UpdateAction
    {
        [JsonProperty(PropertyName = "description")]
        public LocalizedString Description { get; }

        public SetDescriptionAction(LocalizedString description)
        {
            this.Action = "setDescription";
            this.Description = description;
        }
    }
}
