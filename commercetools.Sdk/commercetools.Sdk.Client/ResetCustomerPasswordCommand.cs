using commercetools.Sdk.Domain.Customers;

namespace commercetools.Sdk.Client
{
    public class ResetCustomerPasswordCommand : ResetPasswordCommand<Customer>
    {
        public ResetCustomerPasswordCommand(string tokenValue, string newPassword, int? version = null)
            : base(tokenValue, newPassword, version)
        {
        }
    }
}
