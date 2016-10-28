using System;

namespace commercetools.ProductTypes
{
    /// <summary>
    /// BooleanType
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-productTypes.html#booleantype"/>
    public class BooleanType : AttributeType
    {
        #region Constructors
        /// <summary>
        /// Returns a JSON representation of this instance.
        /// </summary>
        /// <returns>JObject</returns>
        public BooleanType(dynamic data = null)
            : base((object)data)
        {
        }

        #endregion
    }
}