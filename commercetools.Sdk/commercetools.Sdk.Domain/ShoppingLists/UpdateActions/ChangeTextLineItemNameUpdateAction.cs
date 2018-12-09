namespace commercetools.Sdk.Domain.ShoppingLists.UpdateActions
{
    using System.ComponentModel.DataAnnotations;

    public class ChangeTextLineItemNameUpdateAction : UpdateAction<ShoppingList>
    {
        public string Action => "changeTextLineItemName";
        [Required]
        public string TextLineItemId { get; set; }
        [Required]
        public LocalizedString Name { get; set; }
    }
}