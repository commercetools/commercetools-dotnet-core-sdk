using commercetools.Common;
using Newtonsoft.Json;

namespace commercetools.DiscountCodes.UpdateActions
{
    public class SetMaxApplicationsPerCustomer : UpdateAction
    {
        [JsonProperty(PropertyName = "maxApplicationsPerCustomer")]
        public int? MaxApplicationsPerCustomer { get; set; }

        public SetMaxApplicationsPerCustomer()
        {
            this.Action = "setMaxApplicationsPerCustomer";
        }
    }
}
