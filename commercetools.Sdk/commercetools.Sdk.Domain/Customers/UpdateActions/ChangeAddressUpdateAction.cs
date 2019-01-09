using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Customers.UpdateActions
{
    public class ChangeAddressUpdateAction : UpdateAction<Customer>
    {
        public string Action => "changeAddress";
        [Required]
        public string AddressId { get; set; }
        [Required]
        public Address Address { get; set; }
    }
}
