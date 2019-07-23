using commercetools.Sdk.Domain.CartDiscounts;

namespace commercetools.Sdk.Domain.Carts
{
    public class DiscountedLineItemPortion
    {
        public Reference<CartDiscount> Discount { get; set; }
        public BaseMoney DiscountedAmount { get; set; }
    }
}