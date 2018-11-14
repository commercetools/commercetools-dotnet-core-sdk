using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Domain
{
    public class FacetTerm
    {
        public string Term { get; set; }
        public double Count { get; set; }
        public double ProductCount { get; set; }
    }
}
