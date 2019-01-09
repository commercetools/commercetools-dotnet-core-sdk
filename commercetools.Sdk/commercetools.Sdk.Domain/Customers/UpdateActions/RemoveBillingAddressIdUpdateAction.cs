using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Customers.UpdateActions
{
    public class RemoveBillingAddressIdUpdateAction : UpdateAction<Customer>
    {
        public string Action => "removeBillingAddressId";
        [Required]
        public string AddressId { get; set; }
    }
}
