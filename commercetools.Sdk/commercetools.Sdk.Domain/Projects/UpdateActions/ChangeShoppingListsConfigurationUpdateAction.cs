namespace commercetools.Sdk.Domain.Projects.UpdateActions
{
    public class ChangeShoppingListsConfigurationUpdateAction : UpdateAction<Project>
    {
        public string Action => "changeShoppingListsConfiguration";
        public ShoppingListsConfiguration ShoppingListsConfiguration { get; set; }
    }
}
