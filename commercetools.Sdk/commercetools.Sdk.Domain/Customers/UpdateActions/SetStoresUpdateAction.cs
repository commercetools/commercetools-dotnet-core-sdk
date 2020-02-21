using System.Collections.Generic;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Stores;

namespace commercetools.Sdk.Domain.Customers.UpdateActions
{
    public class SetStoresUpdateAction : UpdateAction<Customer>
    {
        public string Action => "setStores";
        public List<IReferenceable<Store>> Stores { get; set; }
    }
}
