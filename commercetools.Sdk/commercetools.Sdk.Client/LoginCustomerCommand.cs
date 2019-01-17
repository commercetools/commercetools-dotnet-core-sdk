using commercetools.Sdk.Domain.Customers;

namespace commercetools.Sdk.Client
{
    public class LoginCustomerCommand : LoginCommand<Customer>
    {
        public LoginCustomerCommand(string email, string password)
            : base(email, password)
        {
        }
    }
}
