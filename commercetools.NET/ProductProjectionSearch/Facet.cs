using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using commercetools.Common;

namespace commercetools.ProductProjectionSearch
{
    /// <summary>
    /// Facets calculate statistical counts to aid in faceted navigation.
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-products-search.html#facets"/>
    public class Facet
    {
        #region Properties

        [JsonProperty(PropertyName = "type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public FacetType? Type { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        protected Facet(dynamic data = null)
        {
            if (data == null)
            {
                return;
            }

            FacetType? type;

            this.Type = Helper.TryGetEnumByEnumMemberAttribute<FacetType?>((string)data.type, out type) ? type : null;
        }

        #endregion
    }
}
