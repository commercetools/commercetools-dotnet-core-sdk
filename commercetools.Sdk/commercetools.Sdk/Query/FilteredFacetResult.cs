using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Domain
{
    [FacetType("filter")]
    public class FilteredFacetResult : FacetResult
    {
        public int Count { get; set; }
        public int ProductCount { get; set; }
    }
}
