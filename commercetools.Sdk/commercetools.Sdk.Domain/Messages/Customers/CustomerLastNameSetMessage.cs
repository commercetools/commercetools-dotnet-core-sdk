using commercetools.Sdk.Domain.Customers;

namespace commercetools.Sdk.Domain.Messages.Customers
{
    [TypeMarker("CustomerLastNameSet")]
    public class CustomerLastNameSetMessage : Message<Customer>
    {
        public string LastName { get; set; }
    }
}
