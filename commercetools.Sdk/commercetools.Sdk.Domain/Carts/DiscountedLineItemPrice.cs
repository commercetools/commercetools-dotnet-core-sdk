using System.Collections.Generic;

namespace commercetools.Sdk.Domain.Carts
{
    public class DiscountedLineItemPrice
    {
        public BaseMoney Value { get; set; }
        public List<DiscountedLineItemPortion> IncludedDiscounts { get; set; }
    }
}