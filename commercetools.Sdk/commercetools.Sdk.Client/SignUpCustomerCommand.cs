using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Customers;

namespace commercetools.Sdk.Client
{
    public class SignUpCustomerCommand : SignUpCommand<Customer>
    {
        public SignUpCustomerCommand(IDraft<Customer> entity)
            : base(entity)
        {
        }
    }
}