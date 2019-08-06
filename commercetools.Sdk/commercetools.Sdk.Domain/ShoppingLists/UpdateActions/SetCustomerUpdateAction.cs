using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Customers;

namespace commercetools.Sdk.Domain.ShoppingLists.UpdateActions
{
    public class SetCustomerUpdateAction : UpdateAction<ShoppingList>
    {
        public string Action => "setCustomer";
        public IReference<Customer> Customer { get; set; }
    }
}
