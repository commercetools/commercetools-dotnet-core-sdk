using commercetools.Sdk.Domain.Customers;

namespace commercetools.Sdk.Domain.Messages.Customers
{
    [TypeMarker("CustomerPasswordUpdated")]
    public class CustomerPasswordUpdatedMessage : Message<Customer>
    {
        public bool Reset { get; set; }
    }
}
