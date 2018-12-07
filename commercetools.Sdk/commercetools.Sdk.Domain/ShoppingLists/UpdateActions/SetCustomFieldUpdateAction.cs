namespace commercetools.Sdk.Domain.ShoppingLists.UpdateActions
{
    using System.ComponentModel.DataAnnotations;

    public class SetCustomFieldUpdateAction : UpdateAction<ShoppingList>
    {
        public string Action => "setCustomField";
        [Required]
        public string Name { get; set; }
        public object Value { get; set; }
    }
}