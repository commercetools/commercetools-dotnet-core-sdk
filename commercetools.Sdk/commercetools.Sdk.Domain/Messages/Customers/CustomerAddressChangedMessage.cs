using commercetools.Sdk.Domain.Customers;

namespace commercetools.Sdk.Domain.Messages.Customers
{
    [TypeMarker("CustomerAddressChanged")]
    public class CustomerAddressChangedMessage : Message<Customer>
    {
        public Address Address { get; set; }
    }
}
