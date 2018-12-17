namespace commercetools.Sdk.Domain
{
    using System.Collections.Generic;

    [TypeMarker("absolute")]
    public class AbsoluteCartDiscountValue : CartDiscountValue
    {
        public List<Money> Money { get; set; }
    }
}