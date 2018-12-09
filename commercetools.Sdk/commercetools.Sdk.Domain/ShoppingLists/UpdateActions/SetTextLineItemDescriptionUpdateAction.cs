namespace commercetools.Sdk.Domain.ShoppingLists.UpdateActions
{
    using System.ComponentModel.DataAnnotations;

    public class SetTextLineItemDescriptionUpdateAction : UpdateAction<ShoppingList>
    {
        public string Action => "setTextLineItemDescription";
        [Required]
        public string TextLineItemId { get; set; }
        public LocalizedString Description { get; set; }
    }
}