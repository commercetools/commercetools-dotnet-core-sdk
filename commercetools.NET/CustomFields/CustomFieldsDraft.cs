using commercetools.Common;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace commercetools.CustomFields
{
    /// <summary>
    /// The representation to be sent to the server when creating a resource with custom fields.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-custom-fields.html#customfieldsdraft"/>
    public class CustomFieldsDraft
    {
        #region Properties

        [JsonProperty(PropertyName = "type")]
        public ResourceIdentifier Type { get; set; }

        [JsonProperty(PropertyName = "fields")]
        public JObject Fields { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public CustomFieldsDraft(ResourceIdentifier type)
        {
            this.Type = type;
        }

        #endregion
    }
}
