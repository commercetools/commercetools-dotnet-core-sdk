using System.Collections.Generic;
using commercetools.Sdk.Domain.Validation.Attributes;

namespace commercetools.Sdk.Domain.TaxCategories
{
    public class TaxRateDraft
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public bool IncludedInPrice { get; set; }
        [Country]
        public string Country { get; set; }
        public string State { get; set; }
        public List<SubRate> SubRates { get; set; }
    }
}
