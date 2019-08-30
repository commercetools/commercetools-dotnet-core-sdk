using commercetools.Sdk.Domain.Customers;

namespace commercetools.Sdk.Domain.Messages.Customers
{
    [TypeMarker("CustomerAddressAdded")]
    public class CustomerAddressAddedMessage : Message<Customer>
    {
        public Address Address { get; set; }
    }
}
