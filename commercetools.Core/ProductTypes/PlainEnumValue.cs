using System;

using Newtonsoft.Json;

namespace commercetools.Common
{
    /// <summary>
    /// PlainEnumValue
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-productTypes.html#plainenumvalue"/>
    public class PlainEnumValue
    {
        #region Properties

        [JsonProperty(PropertyName = "key")]
        public string Key { get; private set; }

        [JsonProperty(PropertyName = "label")]
        public string Label { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public PlainEnumValue(dynamic data = null)
        {
            if (data == null)
            {
                return;
            }

            this.Key = data.key;
            this.Label = data.label;
        }

        #endregion
    }
}