using System;
using System.Collections.Generic;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Types
{
    /// <summary>
    /// EnumValue
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-types.html#enumvalue"/>
    public class EnumValue
    {
        #region Properties

        [JsonProperty(PropertyName = "key")]
        public string Key { get; private set; }

        [JsonProperty(PropertyName = "label")]
        public LocalizedString Label { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Returns a JSON representation of this instance.
        /// </summary>
        /// <returns>JObject</returns>
        public EnumValue(dynamic data = null)
        {
            if (data == null)
            {
                return;
            }

            this.Key = data.key;
            this.Label = new LocalizedString(data.label);
        }

        #endregion
    }
}