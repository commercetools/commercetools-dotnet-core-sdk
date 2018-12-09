namespace commercetools.Sdk.Domain.ShoppingLists.UpdateActions
{
    using System.ComponentModel.DataAnnotations;

    public class SetTextLineItemCustomFieldUpdateAction : UpdateAction<ShoppingList>
    {
        public string Action => "setTextLineItemCustomField";
        [Required]
        public string TextLineItemId { get; set; }
        [Required]
        public string Name { get; set; }
        public object Value { get; set; }
    }
}