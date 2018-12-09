namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class AddDiscountCodeUpdateAction : UpdateAction<Cart>
    {
        public string Action => "addDiscountCode";
        [Required]
        public string Code { get; set; }
    }
}