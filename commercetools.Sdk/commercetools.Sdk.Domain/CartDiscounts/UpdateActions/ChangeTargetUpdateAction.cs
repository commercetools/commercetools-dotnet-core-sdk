using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace commercetools.Sdk.Domain.CartDiscounts
{
    public class ChangeTargetUpdateAction : UpdateAction<CartDiscount>
    {
        public string Action => "changeTarget";
        [Required]
        public CartDiscountTarget Target { get; set; }
    }
}
