namespace commercetools.Sdk.Domain.ShoppingLists.UpdateActions
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ChangeTextLineItemQuantityUpdateAction : UpdateAction<ShoppingList>
    {
        public string Action => "changeTextLineItemQuantity";
        [Required]
        public string TextLineItemId { get; set; }
        public double Quantity { get; set; }
    }
}