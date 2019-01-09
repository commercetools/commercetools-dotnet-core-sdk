using commercetools.Sdk.Domain.Customers;

namespace commercetools.Sdk.Client
{
    public class ChangeCustomerPasswordCommand : ChangePasswordCommand<Customer>
    {
        public ChangeCustomerPasswordCommand(string id, int version, string currentPassword, string newPassword)
            : base(id, version, currentPassword, newPassword)
        {
        }
    }
}