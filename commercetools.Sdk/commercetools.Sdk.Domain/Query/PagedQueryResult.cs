using System.Collections.Generic;

namespace commercetools.Sdk.Domain
{
    public class PagedQueryResult<T>
    {
        public int Offset { get; set; }
        public int Limit { get; set; }
        public int Count { get; set; }
        public int Total { get; set; }
        public Dictionary<string, FacetResult> Facets { get; set; }
        public List<T> Results { get; set; }
    }
}