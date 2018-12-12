namespace commercetools.Sdk.Domain
{
    using System.Collections.Generic;

    [TypeMarker("absolute")]
    public class AbsoluteProductDiscountValue : ProductDiscountValue
    {
        public List<Money> Money { get; set; }
    }
}