using commercetools.Sdk.Domain.Customers;

namespace commercetools.Sdk.Domain.Common
{
    public class ClientLogging
    {
        public string ClientId { get; set; }
        public string ExternalUserId { get; set; }
        public Reference<Customer> Customer { get; set; }
        public string AnonymousId { get; set; }
    }
}