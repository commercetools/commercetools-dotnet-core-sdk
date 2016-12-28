using System;

namespace commercetools.ProductTypes
{
    /// <summary>
    /// DateType
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-productTypes.html#datetype"/>
    public class DateType : AttributeType
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public DateType()
            : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public DateType(dynamic data = null) 
            : base((object)data)
        {
        }

        #endregion
    }
}
