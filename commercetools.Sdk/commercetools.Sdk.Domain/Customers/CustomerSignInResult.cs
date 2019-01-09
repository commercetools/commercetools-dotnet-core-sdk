using commercetools.Sdk.Domain.Carts;

namespace commercetools.Sdk.Domain.Customers
{
    public class CustomerSignInResult : SignInResult<Customer>
    {
        public Customer Customer { get; set; }
        public Cart Cart { get; set; }
    }
}
