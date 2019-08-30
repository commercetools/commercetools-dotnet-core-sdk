using commercetools.Sdk.Domain.Customers;

namespace commercetools.Sdk.Domain.Messages.Customers
{
    [TypeMarker("CustomerCompanyNameSet")]
    public class CustomerCompanyNameSetMessage : Message<Customer>
    {
        public string CompanyName { get; set; }
    }
}
