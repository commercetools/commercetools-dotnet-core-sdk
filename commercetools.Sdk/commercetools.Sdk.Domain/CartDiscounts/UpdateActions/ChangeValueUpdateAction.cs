using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace commercetools.Sdk.Domain.CartDiscounts
{
    public class ChangeValueUpdateAction : UpdateAction<CartDiscount>
    {
        public string Action => "changeValue";
        [Required]
        public CartDiscountValue Value { get; set; }
    }
}
