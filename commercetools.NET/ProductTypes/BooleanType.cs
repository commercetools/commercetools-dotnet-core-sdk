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
        /// Constructor.
        /// </summary>
        public BooleanType()
            : base()
        {
        }

        /// <summary>
        /// Returns a JSON representation of this instance.
        /// </summary>
        /// <returns>JObject</returns>
        public BooleanType(dynamic data)
            : base((object)data)
        {
        }

        #endregion
    }
}
