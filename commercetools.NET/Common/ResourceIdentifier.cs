using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace commercetools.Common
{
    /// <summary>
    /// A reference to a resource can be created by providing the ID of the resource.
    /// </summary>
    /// <remarks>Some resources also use the key as a unique identifier and a reference can be created by providing the key instead of the ID. In this case, the server will find the resource with the given key and use the id of the found resource to create a reference.</remarks>
    /// <see href="http://dev.commercetools.com/http-api-types.html#resourceidentifier"/>
    public class ResourceIdentifier
    {
        #region Properties

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }

        [JsonProperty(PropertyName = "typeId")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ReferenceType? TypeId { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public ResourceIdentifier()
        {
        }

        /// <summary>
        /// Creates an FieldType using JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public ResourceIdentifier(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            ReferenceType? typeId;

            this.Id = data.id;
            this.Key = data.key;
            this.TypeId = Helper.TryGetEnumByEnumMemberAttribute<ReferenceType?>((string)data.typeId, out typeId) ? typeId : null;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            ResourceIdentifier resourceIdentifier = obj as ResourceIdentifier;

            if (resourceIdentifier == null)
            {
                return false;
            }

            return (resourceIdentifier.Id.Equals(this.Id) && resourceIdentifier.Key.Equals(this.Key));
        }

        /// <summary>
        /// GetHashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        #endregion
    }
}
