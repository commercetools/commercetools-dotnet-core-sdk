using System.Collections.Generic;

using commercetools.Common;
using commercetools.Types;

using Newtonsoft.Json;

namespace commercetools.Orders.UpdateActions
{
    /// <summary>
    /// This action sets, overwrites or removes the existing custom type and fields for an existing order LineItem.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#set-lineitem-custom-type"/>
    public class SetLineItemCustomTypeAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Type
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public ResourceIdentifier Type { get; set; }

        /// <summary>
        /// Line item ID
        /// </summary>
        [JsonProperty(PropertyName = "lineItemId")]
        public string LineItemId { get; set; }

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
        /// <param name="lineItemId">Line item ID</param>
        public SetLineItemCustomTypeAction(string lineItemId)
        {
            this.Action = "setLineItemCustomType";
            this.LineItemId = lineItemId;
        }

        #endregion
    }
}
