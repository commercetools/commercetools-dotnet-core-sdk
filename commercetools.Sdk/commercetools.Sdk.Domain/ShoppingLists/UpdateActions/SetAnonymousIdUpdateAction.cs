namespace commercetools.Sdk.Domain.ShoppingLists.UpdateActions
{
    public class SetAnonymousIdUpdateAction : UpdateAction<ShoppingList>
    {
        public string Action => "setAnonymousId";
        public string AnonymousId { get; set; }
    }
}