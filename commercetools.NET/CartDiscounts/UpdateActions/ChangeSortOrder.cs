using commercetools.Common;
using Newtonsoft.Json;

namespace commercetools.CartDiscounts.UpdateActions
{
    public class ChangeSortOrder: UpdateAction
    {
        [JsonProperty(PropertyName = "sortOrder")]
        public string SortOrder { get; }
        public ChangeSortOrder(string sortOrder)
        {
            this.Action = "changeSortOrder";
            this.SortOrder = sortOrder;
        }
    }
}
