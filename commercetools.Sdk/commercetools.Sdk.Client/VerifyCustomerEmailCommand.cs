using commercetools.Sdk.Domain.Customers;

namespace commercetools.Sdk.Client
{
    public class VerifyCustomerEmailCommand : VerifyEmailCommand<Customer>
    {
        public VerifyCustomerEmailCommand(string tokenValue)
            : base(tokenValue)
        {
        }

        public VerifyCustomerEmailCommand(string tokenValue, int version)
            : base(tokenValue, version)
        {
        }
    }
}
