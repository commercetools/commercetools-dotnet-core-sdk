using System;

namespace commercetools.ProductTypes
{
    /// <summary>
    /// DateTimeType
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-productTypes.html#datetimetype"/>
    public class DateTimeType : AttributeType
    {
        #region Constructors

        /// <summary>
        /// Returns a JSON representation of this instance.
        /// </summary>
        /// <returns>JObject</returns>
        public DateTimeType(dynamic data = null)
            : base((object)data)
        {
        }

        #endregion
    }
}
