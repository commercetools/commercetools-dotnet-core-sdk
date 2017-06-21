using commercetools.Common;
using Newtonsoft.Json;

namespace commercetools.DiscountCodes.UpdateActions
{
    public class SetNameAction : UpdateAction
    {
        [JsonProperty(PropertyName = "name")]
        public LocalizedString Name { get; set; }

        public SetNameAction()
        {
            this.Action = "setName";
        }
    }
}
