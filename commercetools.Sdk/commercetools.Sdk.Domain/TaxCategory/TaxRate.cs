using System.Collections.Generic;
using commercetools.Sdk.Domain.Validation.Attributes;

namespace commercetools.Sdk.Domain
{
    public class TaxRate
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public bool IncludedInPrice { get; set; }
        [Country]
        public string Country { get; set; }
        public string State { get; set; }
        public List<SubRate> SubRates { get; set; }
    }
}