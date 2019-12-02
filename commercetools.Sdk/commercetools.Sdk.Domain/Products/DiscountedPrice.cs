using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.ProductDiscounts;

namespace commercetools.Sdk.Domain
{
    public class DiscountedPrice
    {
        public Money Value { get; set; }
        public Reference<ProductDiscount> Discount { get; set; }
    }
}