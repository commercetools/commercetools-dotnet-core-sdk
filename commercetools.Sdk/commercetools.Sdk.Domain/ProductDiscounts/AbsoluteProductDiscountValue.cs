using System.Collections.Generic;

namespace commercetools.Sdk.Domain.ProductDiscounts
{
    [TypeMarker("absolute")]
    public class AbsoluteProductDiscountValue : ProductDiscountValue
    {
        public List<Money> Money { get; set; }
    }
}