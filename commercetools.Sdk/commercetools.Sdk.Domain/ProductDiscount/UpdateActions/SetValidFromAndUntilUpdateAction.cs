using System;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.ProductDiscounts
{
    public class SetValidFromAndUntilUpdateAction : UpdateAction<ProductDiscount>
    {
        public string Action => "setValidFromAndUntil";
        public DateTime ValidFrom { get; set; }
        public DateTime ValidUntil { get; set; }
    }
}