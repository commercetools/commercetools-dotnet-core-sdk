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
        /// Returns a JSON representation of this instance.
        /// </summary>
        /// <returns>JObject</returns>
        public LocalizableTextType(dynamic data = null)
            : base((object)data)
        {
        }

        #endregion
    }
}
