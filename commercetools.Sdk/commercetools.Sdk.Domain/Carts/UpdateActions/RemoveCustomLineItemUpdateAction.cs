namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class RemoveCustomLineItemUpdateAction : UpdateAction<Cart>
    {
        public string Action => "removeCustomLineItem";
        [Required]
        public string CustomLineItemId { get; set; }
    }
}