using commercetools.Common;
using commercetools.Types;

using Newtonsoft.Json;

namespace commercetools.Orders.UpdateActions
{
    /// <summary>
    /// This action sets, overwrites or removes any existing custom field for an existing order LineItem.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#set-lineitem-customfield"/>
    public class SetLineItemCustomFieldAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Line item ID
        /// </summary>
        [JsonProperty(PropertyName = "lineItemId")]
        public string LineItemId { get; set; }

        /// <summary>
        /// Field name
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Field value
        /// </summary>
        [JsonProperty(PropertyName = "value")]
        public FieldType Value { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="lineItemId">Line item ID</param>
        /// <param name="name">Field name</param>
        public SetLineItemCustomFieldAction(string lineItemId, string name)
        {
            this.Action = "setLineItemCustomField";
            this.LineItemId = lineItemId;
            this.Name = name;
        }

        #endregion
    }
}
