using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain.ShoppingLists.UpdateActions
{
    public class SetCustomTypeUpdateAction : UpdateAction<ShoppingList>
    {
        public string Action => "setCustomType";
        public IReference<Type> Type { get; set; }
        public Fields Fields { get; set; }
    }
}
