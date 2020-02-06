using commercetools.Sdk.Domain.Stores;

namespace commercetools.Sdk.Domain.Customers.UpdateActions
{
    public class RemoveStoreUpdateAction : UpdateAction<Customer>
    {
        public string Action => "removeStore";
        public ResourceIdentifier<Store> Store { get; set; }
    }
}
