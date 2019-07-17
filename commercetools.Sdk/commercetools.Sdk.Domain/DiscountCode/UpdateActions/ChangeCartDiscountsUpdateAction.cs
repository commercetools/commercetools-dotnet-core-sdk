using System.Collections.Generic;
using commercetools.Sdk.Domain.CartDiscounts;

namespace commercetools.Sdk.Domain.DiscountCodes
{
    public class ChangeCartDiscountsUpdateAction : UpdateAction<DiscountCode>
    {
        public string Action => "changeCartDiscounts";
        public List<Reference<CartDiscount>> CartDiscounts { get; set; }
    }
}