using System;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.CartDiscounts
{
    public class SetValidUntilUpdateAction : UpdateAction<CartDiscount>
    {
        public string Action => "setValidUntil";
        public DateTime ValidUntil { get; set; }
    }
}