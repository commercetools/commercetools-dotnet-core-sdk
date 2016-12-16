using commercetools.Common;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace commercetools.Categories.UpdateActions
{
    /// <summary>
    /// This action sets or removes the custom type for an existing category.
    /// </summary>
    /// <remarks>
    /// This action overwrites any existing custom type and fields.
    /// </remarks>
    /// <see href="http://dev.commercetools.com/http-api-projects-categories.html#set-custom-type"/>
    public class SetCustomTypeAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// ResourceIdentifier to a Type
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public ResourceIdentifier Type { get; set; }

        /// <summary>
        /// A valid JSON object, based on the FieldDefinitions of the Type.
        /// </summary>
        [JsonProperty(PropertyName = "fields")]
        public JObject Fields { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public SetCustomTypeAction(LocalizedString name)
        {
            this.Action = "setCustomType";
        }

        #endregion
    }
}
