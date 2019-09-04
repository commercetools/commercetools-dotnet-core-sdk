using commercetools.Sdk.Domain.Customers;

namespace commercetools.Sdk.Domain.Messages.Customers
{
    [TypeMarker("CustomerAddressRemoved")]
    public class CustomerAddressRemovedMessage : Message<Customer>
    {
        public Address Address { get; set; }
    }
}
