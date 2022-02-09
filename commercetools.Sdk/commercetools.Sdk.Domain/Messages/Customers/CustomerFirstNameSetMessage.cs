using commercetools.Sdk.Domain.Customers;

namespace commercetools.Sdk.Domain.Messages.Customers
{
    [TypeMarker("CustomerFirstNameSet")]
    public class CustomerFirstNameSetMessage : Message<Customer>
    {
        public string FirstName { get; set; }
    }
}
