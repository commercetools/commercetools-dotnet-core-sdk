using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Customers.UpdateActions
{
    public class AddBillingAddressIdUpdateAction : UpdateAction<Customer>
    {
        public string Action => "addBillingAddressId";
        [Required]
        public string AddressId { get; set; }
    }
}
