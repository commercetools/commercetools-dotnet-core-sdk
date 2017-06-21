using commercetools.Common;
using Newtonsoft.Json;

namespace commercetools.DiscountCodes.UpdateActions
{
    public class SetMaxApplicationsPerCustomerAction : UpdateAction
    {
        [JsonProperty(PropertyName = "maxApplicationsPerCustomer")]
        public int? MaxApplicationsPerCustomer { get; set; }

        public SetMaxApplicationsPerCustomerAction()
        {
            this.Action = "setMaxApplicationsPerCustomer";
        }
    }
}
