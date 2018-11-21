using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Domain
{
    [FacetResultType("range")]
    public class RangeFacetResult : FacetResult
    {
        public List<Range> Ranges { get; set; }
    }
}
