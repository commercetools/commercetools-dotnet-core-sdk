using System;

namespace commercetools.ProductTypes
{
    /// <summary>
    /// NumberType
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-productTypes.html#numbertype"/>
    public class NumberType : AttributeType
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public NumberType()
            : base()
        {
        }

        /// <summary>
        /// Returns a JSON representation of this instance.
        /// </summary>
        /// <returns>JObject</returns>
        public NumberType(dynamic data)
            : base((object)data)
        {
        }

        #endregion
    }
}
