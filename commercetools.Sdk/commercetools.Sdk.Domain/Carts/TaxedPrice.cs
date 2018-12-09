using System.Collections.Generic;

namespace commercetools.Sdk.Domain.Carts
{
    public class TaxedPrice
    {
        public Money TotalNet { get; set; }
        public Money TotalGross { get; set; }
        public List<TaxPortion> TaxPortions { get; set; }
    }
}