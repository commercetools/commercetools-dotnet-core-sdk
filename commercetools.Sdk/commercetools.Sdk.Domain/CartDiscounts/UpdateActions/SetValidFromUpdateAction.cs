using System;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.CartDiscounts
{
    public class SetValidFromUpdateAction : UpdateAction<CartDiscount>
    {
        public string Action => "setValidFrom";
        public DateTime? ValidFrom { get; set; }
    }
}
