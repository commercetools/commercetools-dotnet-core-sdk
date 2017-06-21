using commercetools.Common;
using Newtonsoft.Json;

namespace commercetools.DiscountCodes.UpdateActions
{
    public class SetDescription : UpdateAction
    {
        [JsonProperty(PropertyName = "description")]
        public LocalizedString Description { get; set; }

        public SetDescription()
        {
            this.Action = "setDescription";
        }
    }
}
