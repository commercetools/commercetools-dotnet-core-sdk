namespace commercetools.Sdk.Domain.ShoppingLists.UpdateActions
{
    public class SetDescriptionUpdateAction : UpdateAction<ShoppingList>
    {
        public string Action => "setDescription";
        public LocalizedString Description { get; set; }
    }
}