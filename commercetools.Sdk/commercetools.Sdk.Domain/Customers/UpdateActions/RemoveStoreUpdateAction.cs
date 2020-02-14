using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Stores;

namespace commercetools.Sdk.Domain.Customers.UpdateActions
{
    public class RemoveStoreUpdateAction : UpdateAction<Customer>
    {
        public string Action => "removeStore";
        public IReferenceable<Store> Store { get; set; }
    }
}
