using System.Collections.Generic;

using commercetools.Common;
using commercetools.Types;

using Newtonsoft.Json;

namespace commercetools.Orders.UpdateActions
{
    /// <summary>
    /// This action sets, overwrites or removes the existing custom type and fields for an existing order CustomLineItem.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#set-customlineitem-custom-type"/>
    public class SetCustomLineItemCustomTypeAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Type
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public ResourceIdentifier Type { get; set; }

        /// <summary>
        /// Custom line item ID
        /// </summary>
        [JsonProperty(PropertyName = "customLineItemId")]
        public string CustomLineItemId { get; set; }

        /// <summary>
        /// Fields
        /// </summary>
        [JsonProperty(PropertyName = "fields")]
        public List<FieldType> Fields { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="customLineItemId">Custom line item ID</param>
        public SetCustomLineItemCustomTypeAction(string customLineItemId)
        {
            this.Action = "setCustomLineItemCustomType";
            this.CustomLineItemId = customLineItemId;
        }

        #endregion
    }
}
