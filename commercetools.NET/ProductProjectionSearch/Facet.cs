using System.Collections.Generic;

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

        [JsonProperty(PropertyName = "dataType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public FacetDataType? DataType { get; private set; }

        [JsonProperty(PropertyName = "missing")]
        public int? Missing { get; private set; }

        [JsonProperty(PropertyName = "total")]
        public int? Total { get; private set; }

        [JsonProperty(PropertyName = "other")]
        public int? Other { get; private set; }

        [JsonProperty(PropertyName = "terms")]
        public List<FacetTerm> Terms { get; private set; }

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
            FacetDataType? dataType;

            this.Type = Helper.TryGetEnumByEnumMemberAttribute<FacetType?>((string)data.type, out type) ? type : null;
            this.DataType = Helper.TryGetEnumByEnumMemberAttribute<FacetDataType?>((string)data.dataType, out dataType) ? dataType : null;
            this.Missing = data.missing;
            this.Total = data.total;
            this.Other = data.other;
            this.Terms = Helper.GetListFromJsonArray<FacetTerm>(data.terms);
        }

        #endregion
    }
}
