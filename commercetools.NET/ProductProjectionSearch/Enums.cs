using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace commercetools.ProductProjectionSearch
{
    /// <summary>
    /// List of Facet types.
    /// </summary>
    /// <remarks>
    /// The actual value used for API requests is stored in the EnumMember attribute.
    /// </remarks>
    /// <see href="https://dev.commercetools.com/http-api-projects-products-search.html#facets"/>
    [DataContract]
    public enum FacetType
    {
        [EnumMember(Value = "terms")]
        Term,
        [EnumMember(Value = "range")]
        Range,
        [EnumMember(Value = "filter")]
        Filter
    }

    /// <summary>
    /// Sort direction.
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-products-search.html#sorting"/>
    public enum SortDirection
    {
        Ascending,
        Descending
    }
}