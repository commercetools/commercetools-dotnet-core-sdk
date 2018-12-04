using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace commercetools.Sdk.Domain.CartDiscounts
{
    public class ChangeStackingModeUpdateAction : UpdateAction<CartDiscount>
    {
        public string Action => "changeStackingMode";
        [Required]
        public StackingMode StackingMode { get; set; }
    }
}
