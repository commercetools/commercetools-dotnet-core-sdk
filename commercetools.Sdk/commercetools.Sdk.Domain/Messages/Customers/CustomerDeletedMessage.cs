using commercetools.Sdk.Domain.Customers;

namespace commercetools.Sdk.Domain.Messages.Customers
{
    [TypeMarker("CustomerDeleted")]
    public class CustomerDeletedMessage : Message<Customer>
    {
        public Customer Customer { get; set; }
    }
}
