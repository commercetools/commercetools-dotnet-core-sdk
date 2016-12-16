using System.Collections.Generic;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.ProductTypes
{
    /// <summary>
    /// LocalizableEnumType
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-productTypes.html#localizableenumtype"/>
    public class LocalizableEnumType : AttributeType
    {
        #region Properties

        [JsonProperty(PropertyName = "values")]
        public List<LocalizedEnumValue> Values { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public LocalizableEnumType()
            : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public LocalizableEnumType(dynamic data)
            : base((object)data)
        {
            if (data == null)
            {
                return;
            }

            this.Values = Helper.GetListFromJsonArray<LocalizedEnumValue>(data.values);
        }

        #endregion
    }
}

