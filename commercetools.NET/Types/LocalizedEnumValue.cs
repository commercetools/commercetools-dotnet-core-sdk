using System;
using System.Collections.Generic;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Types
{
    /// <summary>
    /// LocalizedEnumValue
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-types.html#localizedenumvalue"/>
    public class LocalizedEnumValue
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
        public LocalizedEnumValue(dynamic data = null)
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