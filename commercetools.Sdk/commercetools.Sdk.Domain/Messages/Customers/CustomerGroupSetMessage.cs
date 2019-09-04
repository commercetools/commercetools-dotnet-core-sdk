using commercetools.Sdk.Domain.CustomerGroups;
using commercetools.Sdk.Domain.Customers;

namespace commercetools.Sdk.Domain.Messages.Customers
{
    [TypeMarker("CustomerGroupSet")]
    public class CustomerGroupSetMessage : Message<Customer>
    {
        public Reference<CustomerGroup> CustomerGroup { get; set; }
    }
}
