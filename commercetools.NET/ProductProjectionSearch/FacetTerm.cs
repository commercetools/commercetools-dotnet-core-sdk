using Newtonsoft.Json;

namespace commercetools.ProductProjectionSearch
{
    /// <summary>
    /// FacetTerm
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-products-search.html#facetterm"/>
    public class FacetTerm
    {
        #region Properties

        [JsonProperty(PropertyName = "term")]
        public string Term { get; private set; }

        [JsonProperty(PropertyName = "count")]
        public int Count { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public FacetTerm(dynamic data = null)
        {
            if (data == null)
            {
                return;
            }

            this.Term = data.term;
            this.Count = data.count;
        }

        #endregion
    }
}
