using System;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.ProductDiscounts
{
    public class SetValidFromUpdateAction : UpdateAction<ProductDiscount>
    {
        public string Action => "setValidFrom";
        public DateTime ValidFrom { get; set; }
    }
}