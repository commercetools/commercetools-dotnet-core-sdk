namespace commercetools.Sdk.Domain.ShoppingLists.UpdateActions
{
    public class SetCustomerUpdateAction : UpdateAction<ShoppingList>
    {
        public string Action => "setCustomer";
        public ResourceIdentifier Customer { get; set; }
    }
}