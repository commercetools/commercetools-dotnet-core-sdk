using commercetools.Sdk.Domain.Customers;

namespace commercetools.Sdk.Domain.Messages.Customers
{
    [TypeMarker("CustomerEmailChanged")]
    public class CustomerEmailChangedMessage : Message<Customer>
    {
        public string Email { get; set; }
    }
}
