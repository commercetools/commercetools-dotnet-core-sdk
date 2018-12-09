namespace commercetools.Sdk.Domain.ShoppingLists.UpdateActions
{
    using System.ComponentModel.DataAnnotations;

    public class SetLineItemCustomFieldUpdateAction : UpdateAction<ShoppingList>
    {
        public string Action => "setLineItemCustomField";
        [Required]
        public string LineItemId { get; set; }
        [Required]
        public string Name { get; set; }
        public object Value { get; set; }
    }
}