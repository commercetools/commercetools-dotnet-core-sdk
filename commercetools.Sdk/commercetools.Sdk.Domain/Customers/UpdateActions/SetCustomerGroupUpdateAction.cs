using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.CustomerGroups;

namespace commercetools.Sdk.Domain.Customers.UpdateActions
{
    public class SetCustomerGroupUpdateAction : UpdateAction<Customer>
    {
        public string Action => "setCustomerGroup";
        public IReference<CustomerGroup> CustomerGroup { get; set; }
    }
}
