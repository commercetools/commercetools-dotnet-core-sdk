using System.Collections.Generic;

namespace commercetools.Sdk.Domain
{
    [FacetResultType("terms")]
    public class TermFacetResult : FacetResult
    {
        public string DateType { get; set; }
        public int Missing { get; set; }
        public int Total { get; set; }
        public int Other { get; set; }
        public List<FacetTerm> Terms { get; set; }
    }
}