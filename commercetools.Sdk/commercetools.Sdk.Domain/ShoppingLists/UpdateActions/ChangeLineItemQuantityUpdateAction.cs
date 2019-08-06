namespace commercetools.Sdk.Domain.ShoppingLists.UpdateActions
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ChangeLineItemQuantityUpdateAction : UpdateAction<ShoppingList>
    {
        public string Action => "changeLineItemQuantity";
        [Required]
        public string LineItemId { get; set; }
        public long Quantity { get; set; }
    }
}
