using System.Collections.Generic;

namespace commercetools.Sdk.Domain.CartDiscounts
{
    [TypeMarker("fixed")]
    public class FixedCartDiscountValue : CartDiscountValue
    {
        public List<Money> Money { get; set; }
    }
}