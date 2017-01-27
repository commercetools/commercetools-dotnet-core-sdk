using System.Collections.Generic;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Types
{
    /// <summary>
    /// LocalizedEnumType
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-types.html#localizedenumtype"/>
    public class LocalizedEnumType : FieldType
    {
        #region Properties

        [JsonProperty(PropertyName = "values")]
        public List<LocalizedEnumValue> Values { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public LocalizedEnumType()
            : base()
        {
            this.Name = "LocalizedEnum";
        }

        /// <summary>
        /// Returns a JSON representation of this instance.
        /// </summary>
        /// <returns>JObject</returns>
        public LocalizedEnumType(dynamic data)
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
