using commercetools.Sdk.Domain.Stores;

namespace commercetools.Sdk.Domain.ShoppingLists.UpdateActions
{
    public class SetStoreUpdateAction : UpdateAction<ShoppingList>
    {
        public string Action => "setStore";
        public ResourceIdentifier<Store> Store{ get; set; }
    }
}