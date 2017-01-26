using System.Collections.Generic;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Types
{
    /// <summary>
    /// EnumType
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-types.html#enumtype"/>
    public class EnumType : FieldType
    {
        #region Properties

        [JsonProperty(PropertyName = "values")]
        public List<EnumValue> Values { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public EnumType()
            : base()
        {
            this.Name = "Enum";
        }

        /// <summary>
        /// Returns a JSON representation of this instance.
        /// </summary>
        /// <returns>JObject</returns>
        public EnumType(dynamic data)
            : base((object)data)
        {
            if (data == null)
            {
                return;
            }

            this.Values = Helper.GetListFromJsonArray<EnumValue>(data.values);
        }

        #endregion
    }
}
