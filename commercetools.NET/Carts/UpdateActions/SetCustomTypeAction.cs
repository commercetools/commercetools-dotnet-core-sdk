using commercetools.Common;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace commercetools.Carts.UpdateActions
{
    /// <summary>
    /// This action sets, overwrites or removes any existing custom type and fields for an existing cart.
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-carts.html#set-custom-type"/>
    public class SetCustomTypeAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// ResourceIdentifier to a Type
        /// </summary>
        /// <remarks>
        /// If set, the custom type is set to this new value. If absent, the custom type and any existing custom fields are removed.
        /// </remarks>
        [JsonProperty(PropertyName = "type")]
        public ResourceIdentifier Type { get; set; }

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
        public SetCustomTypeAction()
        {
            this.Action = "setCustomType";
        }

        #endregion
    }
}
