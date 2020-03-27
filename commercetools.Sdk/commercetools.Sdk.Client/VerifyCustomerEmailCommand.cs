using commercetools.Sdk.Domain.Customers;

namespace commercetools.Sdk.Client
{
    public class VerifyCustomerEmailCommand : VerifyEmailCommand<Customer>
    {
        public VerifyCustomerEmailCommand(string tokenValue, int? version = null)
            : base(tokenValue, version)
        {
        }
    }
}
