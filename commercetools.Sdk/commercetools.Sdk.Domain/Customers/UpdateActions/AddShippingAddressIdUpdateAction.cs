using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Customers.UpdateActions
{
    public class AddShippingAddressIdUpdateAction : UpdateAction<Customer>
    {
        public string Action => "addShippingAddressId";
        [Required]
        public string AddressId { get; set; }
    }
}
