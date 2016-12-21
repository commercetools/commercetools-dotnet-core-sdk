using System.Collections.Generic;

using commercetools.Common;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace commercetools.Products
{
    /// <summary>
    /// SearchKeywords
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-products.html#searchkeywords"/>
    public class SearchKeywords
    {
        #region Properties

        [JsonProperty(PropertyName = "values")]
        public Dictionary<string, List<SearchKeyword>> Values { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public SearchKeywords()
        {
            this.Values = new Dictionary<string, List<SearchKeyword>>();
        }

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public SearchKeywords(dynamic data)
        {
            this.Values = new Dictionary<string, List<SearchKeyword>>();

            if (data == null)
            {
                return;
            }

            foreach (JProperty item in data)
            {
                List<SearchKeyword> value = Helper.GetListFromJsonArray<SearchKeyword>(data[item.Name]);
                SetValue(item.Name, value);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets a value for a particular language.
        /// </summary>
        /// <remarks>If a previous value has been set for the language, it will be overwritten</remarks>
        /// <param name="language">Language</param>
        /// <param name="value">Value</param>
        public void SetValue(string language, List<SearchKeyword> value)
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
        /// <param name="language">Language</param>
        /// <returns>Value, or a null if not set</returns>
        public List<SearchKeyword> GetValue(string language)
        {
            if (this.Values.ContainsKey(language))
            {
                return this.Values[language];
            }

            return null;
        }

        #endregion
    }
}
