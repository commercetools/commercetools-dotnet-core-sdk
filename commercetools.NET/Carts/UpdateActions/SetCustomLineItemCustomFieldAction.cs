using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Carts.UpdateActions
{
    /// <summary>
    /// This action sets, overwrites or removes any existing custom field for an existing CustomLineItem.
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-carts.html#set-customlineitem-customfield"/>
    public class SetCustomLineItemCustomFieldAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Custom line item ID
        /// </summary>
        [JsonProperty(PropertyName = "customLineItemId")]
        public string CustomLineItemId { get; set; }

        /// <summary>
        /// Field name
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Field value
        /// </summary>
        /// <remarks>
        /// If absent or null, this field is removed if it exists.
        /// </remarks>
        [JsonProperty(PropertyName = "value")]
        public object Value { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="customLineItemId">Custom line item ID</param>
        /// <param name="name">Field name</param>
        public SetCustomLineItemCustomFieldAction(string customLineItemId, string name)
        {
            this.Action = "setCustomLineItemCustomField";
            this.CustomLineItemId = customLineItemId;
            this.Name = name;
        }

        #endregion
    }
}
