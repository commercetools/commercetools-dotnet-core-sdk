using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Customers.UpdateActions
{
    public class RemoveAddressUpdateAction : UpdateAction<Customer>
    {
        public string Action => "removeAddress";
        [Required]
        public string AddressId { get; set; }
    }
}
