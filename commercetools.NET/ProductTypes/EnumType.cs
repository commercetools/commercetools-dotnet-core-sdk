using System.Collections.Generic;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.ProductTypes
{
    /// <summary>
    /// EnumType
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-productTypes.html#enumtype"/>
    public class EnumType : AttributeType
    {
        #region Properties

        [JsonProperty(PropertyName = "values")]
        public List<PlainEnumValue> Values { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public EnumType()
            : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EnumType(dynamic data)
            : base((object)data)
        {
            if (data == null)
            {
                return;
            }

            this.Values = Helper.GetListFromJsonArray<PlainEnumValue>(data.values);
        }

        #endregion
    }
}
