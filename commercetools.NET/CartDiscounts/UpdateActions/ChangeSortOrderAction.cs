using commercetools.Common;
using Newtonsoft.Json;

namespace commercetools.CartDiscounts.UpdateActions
{
    public class ChangeSortOrderAction: UpdateAction
    {
        [JsonProperty(PropertyName = "sortOrder")]
        public string SortOrder { get; }
        public ChangeSortOrderAction(string sortOrder)
        {
            this.Action = "changeSortOrder";
            this.SortOrder = sortOrder;
        }
    }
}
