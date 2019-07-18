using commercetools.Sdk.Domain.DiscountCodes;

namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class RemoveDiscountCodeUpdateAction : UpdateAction<Cart>
    {
        public string Action => "removeDiscountCode";
        [Required]
        public Reference<DiscountCode> DiscountCode { get; set; }
    }
}