using System.Collections.Generic;

namespace commercetools.Sdk.Domain
{
    [TypeMarker("terms")]
    public class TermFacetResult : FacetResult
    {
        public string DateType { get; set; }
        public long Missing { get; set; }
        public long Total { get; set; }
        public long Other { get; set; }
        public List<FacetTerm> Terms { get; set; }
    }
}
