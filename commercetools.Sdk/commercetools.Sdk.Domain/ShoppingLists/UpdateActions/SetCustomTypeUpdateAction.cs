namespace commercetools.Sdk.Domain.ShoppingLists.UpdateActions
{
    public class SetCustomTypeUpdateAction : UpdateAction<ShoppingList>
    {
        public string Action => "setCustomType";
        public ResourceIdentifier Type { get; set; }
        public Fields Fields { get; set; }
    }
}