using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Customers;

namespace commercetools.Sdk.Client
{
    public class CreateTokenForCustomerPasswordResetCommand : CreateTokenForPasswordResetCommand<Customer>
    {
        public CreateTokenForCustomerPasswordResetCommand(string email)
            : base(email)
        {
        }

        public CreateTokenForCustomerPasswordResetCommand(string email, int timeToLiveMinutes)
            : base(email, timeToLiveMinutes)
        {
        }
    }
}
