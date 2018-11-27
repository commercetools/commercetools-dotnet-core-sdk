using System.Collections.Generic;

namespace commercetools.Sdk.Domain
{
    [FacetResultType("range")]
    public class RangeFacetResult : FacetResult
    {
        public List<Range> Ranges { get; set; }
    }
}