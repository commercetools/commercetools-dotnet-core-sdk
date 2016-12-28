using System;

using Newtonsoft.Json;

namespace commercetools.ProductTypes
{
    /// <summary>
    /// All attribute types have a name. Some have additional fields such as values in enums or elementType in sets.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-productTypes.html#attributetype"/>
    public abstract class AttributeType
    {
        #region Properties

        [JsonProperty(PropertyName = "name")]
        public string Name { get; protected set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public AttributeType()
        {
        }

        /// <summary>
        /// Returns a JSON representation of this instance.
        /// </summary>
        /// <returns>JObject</returns>
        protected AttributeType(dynamic data)
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
