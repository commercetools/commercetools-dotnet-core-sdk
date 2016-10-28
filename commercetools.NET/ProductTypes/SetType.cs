using System;

using Newtonsoft.Json;

namespace commercetools.ProductTypes
{
    /// <summary>
    /// SetType
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-productTypes.html#settype"/>
    public class SetType : AttributeType
    {
        #region Properties

        [JsonProperty(PropertyName = "elementType")]
        public AttributeType ElementType { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Returns a JSON representation of this instance.
        /// </summary>
        /// <returns>JObject</returns>
        public SetType(dynamic data = null)
            : base((object)data)
        {
            if (data == null)
            {
                return;
            }

            this.ElementType = AttributeTypeFactory.Create(data.elementType);
        }

        #endregion
    }
}