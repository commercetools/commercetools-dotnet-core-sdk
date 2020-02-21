using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Stores;

namespace commercetools.Sdk.Domain.Customers.UpdateActions
{
    public class AddStoreUpdateAction : UpdateAction<Customer>
    {
        public string Action => "addStore";
        public IReferenceable<Store> Store { get; set; }
    }
}
