using System;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.ProdCartDiscountsuctDiscounts
{
    public class SetValidUntilUpdateAction : UpdateAction<CartDiscount>
    {
        public string Action => "setValidUntil";
        public DateTime ValidUntil { get; set; }
    }
}