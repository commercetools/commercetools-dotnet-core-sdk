using commercetools.Common;
using Newtonsoft.Json;

namespace commercetools.CartDiscount.UpdateActions
{
    public class SetDescription: UpdateAction
    {
        [JsonProperty(PropertyName = "description")]
        public LocalizedString Description { get; }

        public SetDescription(LocalizedString description)
        {
            this.Action = "setDescription";
            this.Description = description;
        }
    }
}
