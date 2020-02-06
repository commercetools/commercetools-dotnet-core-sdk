using commercetools.Sdk.Domain.Stores;

namespace commercetools.Sdk.Domain.Customers.UpdateActions
{
    public class AddStoreUpdateAction : UpdateAction<Customer>
    {
        public string Action => "addStore";
        public ResourceIdentifier<Store> Store { get; set; }
    }
}
