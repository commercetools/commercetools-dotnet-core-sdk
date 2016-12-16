using Newtonsoft.Json;

namespace commercetools.Types
{
    /// <summary>
    /// All field types have a name. Some have additional fields such as values in enums or elementType in sets.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-types.html#fieldtype"/>
    public class FieldType
    {
        #region Properties

        [JsonProperty(PropertyName = "name")]
        public string Name { get; protected set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public FieldType()
        {
        }

        /// <summary>
        /// Returns a JSON representation of this instance.
        /// </summary>
        /// <returns>JObject</returns>
        protected FieldType(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            this.Name = data.name;
        }

        #endregion
    }
}
