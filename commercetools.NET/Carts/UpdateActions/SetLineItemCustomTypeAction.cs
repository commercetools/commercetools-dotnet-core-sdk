using System.Collections.Generic;

using commercetools.Common;
using commercetools.Types;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace commercetools.Carts.UpdateActions
{
    /// <summary>
    /// This action sets, overwrites or removes the custom type and fields for an existing LineItem.
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-carts.html#set-lineitem-custom-type"/>
    public class SetLineItemCustomTypeAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// ResourceIdentifier to a Type
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public ResourceIdentifier Type { get; set; }

        /// <summary>
        /// Line item ID
        /// </summary>
        [JsonProperty(PropertyName = "lineItemId")]
        public string LineItemId { get; set; }

        /// <summary>
        /// A valid JSON object, based on the FieldDefinitions of the Type 
        /// </summary>
        /// <remarks>
        /// If set, the custom fields are set to this new value.
        /// </remarks>
        [JsonProperty(PropertyName = "fields")]
        public JObject Fields { get; set; }

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
