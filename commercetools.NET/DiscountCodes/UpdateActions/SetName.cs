using commercetools.Common;
using Newtonsoft.Json;

namespace commercetools.DiscountCodes.UpdateActions
{
    public class SetName : UpdateAction
    {
        [JsonProperty(PropertyName = "name")]
        public LocalizedString Name { get; set; }

        public SetName()
        {
            this.Action = "setName";
        }
    }
}
