using commercetools.Sdk.Domain.DiscountCodes;

namespace commercetools.Sdk.Domain.Carts
{
    public class DiscountCodeInfo
    {
        public Reference<DiscountCode> DiscountCode { get; set; }
        public DiscountCodeState State { get; set; }
    }
}