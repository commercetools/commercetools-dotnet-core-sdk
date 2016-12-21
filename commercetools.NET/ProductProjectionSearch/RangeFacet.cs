using System.Collections.Generic;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.ProductProjectionSearch
{
    /// <summary>
    /// The range facet type counts the ProductVariants for which the query value is in the Facet Query Parameter for Range type range specified in the query.
    /// </summary>
    /// <remarks>
    /// Range type facets are typically used to determine the minimum and maximum value for e.g. product prices to filter products by price with a range slider.
    /// </remarks>
    /// <see href="https://dev.commercetools.com/http-api-projects-products-search.html#range-type"/>
    public class RangeFacet : Facet
    {
        #region Properties

        [JsonProperty(PropertyName = "ranges")]
        public List<Range> Ranges { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public RangeFacet(dynamic data = null)
            : base((object)data)
        {
            if (data == null)
            {
                return;
            }

            this.Ranges = Helper.GetListFromJsonArray<Range>(data.ranges);
        }

        #endregion
    }
}
