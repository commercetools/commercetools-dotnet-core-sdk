using commercetools.Sdk.Domain.Customers;

namespace commercetools.Sdk.Client
{
    public class CreateTokenForCustomerEmailVerificationCommand : CreateTokenForEmailVerificationCommand<Customer>
    {
        public CreateTokenForCustomerEmailVerificationCommand(string id, int timeToLiveMinutes)
            : base(id, timeToLiveMinutes)
        {
        }

        public CreateTokenForCustomerEmailVerificationCommand(string id, int timeToLiveMinutes, int version)
            : base(id, timeToLiveMinutes, version)
        {
        }
    }
}
