namespace commercetools.Sdk.Domain.ShoppingLists.UpdateActions
{
    using System.ComponentModel.DataAnnotations;

    public class RemoveTextLineItemUpdateAction : UpdateAction<ShoppingList>
    {
        public string Action => "removeTextLineItem";
        [Required]
        public string TextLineItemId { get; set; }
        public long Quantity { get; set; }
    }
}
