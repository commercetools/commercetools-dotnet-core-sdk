using System;

namespace commercetools.ProductTypes
{
    /// <summary>
    /// TimeType
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-productTypes.html#timetype"/>
    public class TimeType : AttributeType
    {
        #region Constructors

        /// <summary>
        /// Returns a JSON representation of this instance.
        /// </summary>
        /// <returns>JObject</returns>
        public TimeType(dynamic data = null)
            : base((object)data)
        {
        }

        #endregion
    }
}