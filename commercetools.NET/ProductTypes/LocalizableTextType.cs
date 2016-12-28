using System;

namespace commercetools.ProductTypes
{
    /// <summary>
    /// LocalizableTextType
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-productTypes.html#localizabletexttype"/>
    public class LocalizableTextType : AttributeType
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public LocalizableTextType()
            : base()
        {
        }

        /// <summary>
        /// Returns a JSON representation of this instance.
        /// </summary>
        /// <returns>JObject</returns>
        public LocalizableTextType(dynamic data)
            : base((object)data)
        {
        }

        #endregion
    }
}
