using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.CartDiscounts;

namespace commercetools.Sdk.Domain.DiscountCodes.UpdateActions
{
    public class ChangeCartDiscountsUpdateAction : UpdateAction<DiscountCode>
    {
        public string Action => "changeCartDiscounts";
        [Required]
        public List<Reference<CartDiscount>> CartDiscounts { get; set; }
    }
}
