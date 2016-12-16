using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.ProductTypes
{
    /// <summary>
    /// NestedType
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-productTypes.html#nestedtype"/>
    public class NestedType : AttributeType
    {
        #region Properties

        [JsonProperty(PropertyName = "typeReference")]
        public Reference TypeReference { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public NestedType()
            : base()
        {
        }

        /// <summary>
        /// Returns a JSON representation of this instance.
        /// </summary>
        /// <returns>JObject</returns>
        public NestedType(dynamic data)
            : base((object)data)
        {
            if (data == null)
            {
                return;
            }

            this.TypeReference = new Reference(data.typeReference);
        }

        #endregion
    }
}
