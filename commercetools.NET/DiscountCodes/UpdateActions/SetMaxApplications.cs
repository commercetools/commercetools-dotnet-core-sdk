using commercetools.Common;
using Newtonsoft.Json;

namespace commercetools.DiscountCodes.UpdateActions
{
    public class SetMaxApplications : UpdateAction
    {
        [JsonProperty(PropertyName = "maxApplications")]
        public int? MaxApplications { get; set; }

        public SetMaxApplications()
        {
            this.Action = "setMaxApplications";
        }
    }
}
