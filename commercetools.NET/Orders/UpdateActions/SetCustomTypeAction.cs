using commercetools.Common;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace commercetools.Orders.UpdateActions
{
    /// <summary>
    /// This action sets, overwrites or removes any existing custom type and fields for an existing order.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#set-custom-type"/>
    public class SetCustomTypeAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Type
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public ResourceIdentifier Type { get; set; }

        /// <summary>
        /// A valid JSON object, based on the FieldDefinitions of the Type 
        /// </summary>
        [JsonProperty(PropertyName = "fields")]
        public JObject Fields { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public SetCustomTypeAction()
        {
            this.Action = "setCustomType";
        }

        #endregion
    }
}
