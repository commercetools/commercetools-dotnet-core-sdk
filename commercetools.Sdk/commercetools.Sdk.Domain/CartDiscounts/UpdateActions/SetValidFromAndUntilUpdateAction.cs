using System;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.CartDiscounts
{
    public class SetValidFromAndUntilUpdateAction : UpdateAction<CartDiscount>
    {
        public string Action => "setValidFromAndUntil";
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidUntil { get; set; }
    }
}
