using commercetools.Sdk.Domain.Customers;

namespace commercetools.Sdk.Domain.Messages.Customers
{
    [TypeMarker("CustomerTitleSet")]
    public class CustomerTitleSetMessage : Message<Customer>
    {
        public string Title { get; set; }
    }
}
