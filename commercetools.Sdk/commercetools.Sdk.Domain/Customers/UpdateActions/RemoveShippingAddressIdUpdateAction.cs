using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Customers.UpdateActions
{
    public class RemoveShippingAddressIdUpdateAction : UpdateAction<Customer>
    {
        public string Action => "removeShippingAddressId";
        [Required]
        public string AddressId { get; set; }
    }
}
