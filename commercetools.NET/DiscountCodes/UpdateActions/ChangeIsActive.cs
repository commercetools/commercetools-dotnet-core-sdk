using commercetools.Common;
using Newtonsoft.Json;

namespace commercetools.DiscountCodes.UpdateActions
{
    public class ChangeIsActive : UpdateAction
    {
        [JsonProperty(PropertyName = "isActive")]
        public bool IsActive { get; private set; }

        public ChangeIsActive(bool isActive)
        {
            this.Action = "changeIsActive";
            this.IsActive = isActive;
        }
    }
}
