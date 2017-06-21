using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace commercetools.Common
{
    /// <summary>
    /// A JSON object representing a (loose) reference to another resource on the commercetools platform.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-types.html#reference"/>
    public class Reference
    {
        #region Properties

        [JsonProperty(PropertyName = "typeId")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ReferenceType? ReferenceType { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public Reference()
        {
        }

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public Reference(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            ReferenceType? referenceType;

            this.ReferenceType = Helper.TryGetEnumByEnumMemberAttribute<ReferenceType?>((string)data.typeId, out referenceType) ? referenceType : null;
            this.Id = data.id;
        }

        #endregion
    }
}
