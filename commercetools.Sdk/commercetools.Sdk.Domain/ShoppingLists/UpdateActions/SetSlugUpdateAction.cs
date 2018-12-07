namespace commercetools.Sdk.Domain.ShoppingLists.UpdateActions
{
    public class SetSlugUpdateAction : UpdateAction<ShoppingList>
    {
        public string Action => "setSlug";
        public LocalizedString Slug { get; set; }
    }
}