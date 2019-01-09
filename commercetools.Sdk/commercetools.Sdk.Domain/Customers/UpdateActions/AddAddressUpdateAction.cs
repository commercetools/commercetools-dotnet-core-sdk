using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Customers.UpdateActions
{
    public class AddAddressUpdateAction : UpdateAction<Customer>
    {
        public string Action => "addAddress";
        [Required]
        public Address Address { get; set; }
    }
}
