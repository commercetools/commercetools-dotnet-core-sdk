using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Customers;

namespace commercetools.Sdk.Client
{
    public class ChangeCustomerPasswordCommand : ChangePasswordCommand<Customer>
    {
        public ChangeCustomerPasswordCommand(string id, int version, string currentPassword, string newPassword)
            : base(id, version, currentPassword, newPassword)
        {
        }

        public ChangeCustomerPasswordCommand(Customer customer, string currentPassword, string newPassword)
            : base(customer, currentPassword, newPassword)
        {
        }
    }
}