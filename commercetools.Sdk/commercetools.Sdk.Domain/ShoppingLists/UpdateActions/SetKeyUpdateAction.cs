namespace commercetools.Sdk.Domain.ShoppingLists.UpdateActions
{
    public class SetKeyUpdateAction : UpdateAction<ShoppingList>
    {
        public string Action => "setKey";
        public string Key { get; set; }
    }
}