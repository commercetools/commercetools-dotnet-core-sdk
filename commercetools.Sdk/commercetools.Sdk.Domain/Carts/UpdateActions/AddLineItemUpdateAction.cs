namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public abstract class AddLineItemUpdateAction : UpdateAction<Cart>
    {
        public string Action => "addLineItem";
        [Required]
        public LineItemDraft LineItem { get; set; }
        
        public int Quantity { get; set; }
    }
}