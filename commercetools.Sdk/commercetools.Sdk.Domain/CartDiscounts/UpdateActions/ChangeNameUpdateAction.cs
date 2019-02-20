using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace commercetools.Sdk.Domain.CartDiscounts
{
    public class ChangeNameUpdateAction : UpdateAction<CartDiscount>
    {
        public string Action => "changeName";
        [Required]
        public LocalizedString Name { get; set; }
    }
}
