using System.Collections.Generic;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.ProductProjectionSearch
{
    /// <summary>
    /// The term type facets provide the counts for each of the different values the query parameter happens to have.
    /// </summary>
    /// <remarks>
    /// This is useful for, e.g., obtaining all possible values of a product attribute to provide filters for those values on the frontend.
    /// </remarks>
    /// <see href="https://dev.commercetools.com/http-api-projects-products-search.html#term-type"/>
    public class TermFacet : Facet
    {
        #region Properties

        [JsonProperty(PropertyName = "dataType")]
        public string DataType { get; private set; }

        [JsonProperty(PropertyName = "missing")]
        public int Missing { get; private set; }

        [JsonProperty(PropertyName = "total")]
        public int Total { get; private set; }

        [JsonProperty(PropertyName = "other")]
        public int Other { get; private set; }

        [JsonProperty(PropertyName = "terms")]
        public List<FacetTerm> Terms { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public TermFacet(dynamic data)
            : base((object)data)
        {
            if (data == null)
            {
                return;
            }

            this.DataType = data.dataType;
            this.Missing = data.missing;
            this.Total = data.total;
            this.Other = data.other;
            this.Terms = Helper.GetListFromJsonArray<FacetTerm>(data.terms);
        }

        #endregion
    }
}
