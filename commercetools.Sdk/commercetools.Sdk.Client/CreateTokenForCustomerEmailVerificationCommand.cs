using commercetools.Sdk.Domain.Customers;

namespace commercetools.Sdk.Client
{
    public class CreateTokenForCustomerEmailVerificationCommand : CreateTokenForEmailVerificationCommand<Customer>
    {
       public CreateTokenForCustomerEmailVerificationCommand(string id, int timeToLiveMinutes, int? version = null)
            : base(id, timeToLiveMinutes, version)
        {
        }
    }
}
