using System.Collections.Generic;

namespace commercetools.Sdk.Domain
{
    public class TaxRateDraft
    {
        public string Name { get; set; }
        public double Amount { get; set; }
        public bool IncludedInPrice { get; set; }
        [Country]
        public string Country { get; set; }
        public string State { get; set; }
        public List<SubRate> SubRates { get; set; }
    }
}