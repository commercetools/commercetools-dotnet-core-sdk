using System.Collections.Generic;

namespace commercetools.Sdk.Domain.CartDiscounts
{
    [TypeMarker("absolute")]
    public class AbsoluteCartDiscountValue : CartDiscountValue
    {
        public List<Money> Money { get; set; }
    }
}