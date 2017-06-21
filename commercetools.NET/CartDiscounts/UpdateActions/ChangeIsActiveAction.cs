using commercetools.Common;
using Newtonsoft.Json;

namespace commercetools.CartDiscounts.UpdateActions
{
    public class ChangeIsActiveAction: UpdateAction
    {
        [JsonProperty(PropertyName = "isActive")]
        public bool IsActive { get; }

        public ChangeIsActiveAction(bool isActive)
        {
            this.Action = "changeIsActive";
            this.IsActive = isActive;
        }
    }
}
