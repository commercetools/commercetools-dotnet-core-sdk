using System.Collections.Generic;

using commercetools.Common.Converters;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace commercetools.Common
{
    /// <summary>
    /// A localized string is a JSON object where the keys are of IETF language tag, and the values the corresponding strings used for that language.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-types.html#localizedstring"/>
    [JsonConverter(typeof(LocalizedStringConverter))]
    public class LocalizedString
    {
        #region Properties

        /// <summary>
        /// Values
        /// </summary>
        [JsonExtensionData]
        public Dictionary<string, object> Values { get; private set; }

        /// <summary>
        /// Gets or sets the value for a language.
        /// </summary>
        /// <param name="language">Language in the format of an IETF language tag</param>
        public string this[string language]
        {
            get
            {
                return GetValue(language);
            }
            set
            {
                SetValue(language, value);
            }
        }

        #endregion 

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public LocalizedString()
        {
            this.Values = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public LocalizedString(dynamic data)
        {
            this.Values = new Dictionary<string, object>();

            if (data == null)
            {
                return;
            }

            foreach (JProperty item in data)
            {
                SetValue(item.Name, item.Value.ToString());
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets a value for a particular language.
        /// </summary>
        /// <remarks>If a previous value has been set for the language, it will be overwritten</remarks>
        /// <param name="language">Language in the format of an IETF language tag</param>
        /// <param name="value">Value</param>
        public void SetValue(string language, string value)
        {
            if (this.Values.ContainsKey(language))
            {
                this.Values[language] = value;
            }
            else
            {
                this.Values.Add(language, value);
            }
        }

        /// <summary>
        /// Gets the value for a language.
        /// </summary>
        /// <param name="language">Language in the format of an IETF language tag</param>
        /// <returns>Value, or an empty string if not set</returns>
        public string GetValue(string language)
        {
            if (this.Values.ContainsKey(language))
            {
                return this.Values[language].ToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// Checks if there are any values in this instance.
        /// </summary>
        /// <returns>True if there are values, false otherwise</returns>
        public bool IsEmpty()
        {
            return (this.Values == null || this.Values.Count < 1);
        }

        #endregion
    }
}
