using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Customers;

namespace commercetools.Sdk.Domain.Reviews
{
    public class SetCustomerUpdateAction : UpdateAction<Review>
    {
        public string Action => "setCustomer";
        public IReference<Customer> Customer { get; set; }
    }
}
