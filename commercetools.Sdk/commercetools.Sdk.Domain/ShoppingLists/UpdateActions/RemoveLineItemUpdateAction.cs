namespace commercetools.Sdk.Domain.ShoppingLists.UpdateActions
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class RemoveLineItemUpdateAction : UpdateAction<ShoppingList>
    {
        public string Action => "removeLineItem";
        [Required]
        public string LineItemId { get; set; }
        public double Quantity { get; set; }
    }
}