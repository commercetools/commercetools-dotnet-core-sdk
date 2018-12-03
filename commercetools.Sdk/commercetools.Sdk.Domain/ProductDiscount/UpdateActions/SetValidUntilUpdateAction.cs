using System;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.ProductDiscounts
{
    public class SetValidUntilUpdateAction : UpdateAction<ProductDiscount>
    {
        public string Action => "setValidUntil";
        public DateTime ValidUntil { get; set; }
    }
}