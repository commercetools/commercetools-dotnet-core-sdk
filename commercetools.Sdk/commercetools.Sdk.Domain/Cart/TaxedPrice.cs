using System.Collections.Generic;

namespace commercetools.Sdk.Domain
{
    public class TaxedPrice
    {
        public CentPrecisionMoney TotalNet { get; set; }
        public CentPrecisionMoney TotalGross { get; set; }
        public List<TaxPortion> TaxPortions { get; set; }
    }
}