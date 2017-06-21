using commercetools.Common;
using Newtonsoft.Json;

namespace commercetools.DiscountCodes.UpdateActions
{
    public class SetDescriptionAction : UpdateAction
    {
        [JsonProperty(PropertyName = "description")]
        public LocalizedString Description { get; set; }

        public SetDescriptionAction()
        {
            this.Action = "setDescription";
        }
    }
}
