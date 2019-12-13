using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.DiscountCodes;

namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class RemoveDiscountCodeUpdateAction : UpdateAction<Cart>
    {
        public string Action => "removeDiscountCode";
        [Required]
        public IReference<DiscountCode> DiscountCode { get; set; }
    }
}