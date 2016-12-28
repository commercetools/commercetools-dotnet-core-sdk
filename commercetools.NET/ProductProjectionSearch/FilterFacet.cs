using Newtonsoft.Json;

namespace commercetools.ProductProjectionSearch
{
    /// <summary>
    /// This facet type provides counts for specific values of ProductVariant fields.
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-products-search.html#filter-type"/>
    public class FilterFacet : Facet
    {
        #region Properties

        [JsonProperty(PropertyName = "count")]
        public int Count { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public FilterFacet(dynamic data)
            : base((object)data)
        {
            if (data == null)
            {
                return;
            }

            this.Count = data.count;
        }

        #endregion
    }
}
