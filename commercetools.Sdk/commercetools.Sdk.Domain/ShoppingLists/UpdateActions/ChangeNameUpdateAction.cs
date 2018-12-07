namespace commercetools.Sdk.Domain.ShoppingLists.UpdateActions
{
    using System.ComponentModel.DataAnnotations;

    public class ChangeNameUpdateAction : UpdateAction<ShoppingList>
    {
        public string Action => "changeName";
        [Required]
        public LocalizedString Name { get; set; }
    }
}