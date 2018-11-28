using System.Collections.Generic;

namespace commercetools.Sdk.Domain
{
    [TypeMarker("range")]
    public class RangeFacetResult : FacetResult
    {
        public List<Range> Ranges { get; set; }
    }
}