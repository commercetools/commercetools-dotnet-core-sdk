using commercetools.Sdk.Domain.Customers;

namespace commercetools.Sdk.Domain.Messages.Customers
{
    [TypeMarker("CustomerCreated")]
    public class CustomerCreatedMessage : Message<Customer>
    {
        public Customer Customer { get; set; }
    }
}
