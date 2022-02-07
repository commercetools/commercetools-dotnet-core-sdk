using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Customers.UpdateActions
{
    public class SetAddressCustomFieldUpdateAction : UpdateAction<Customer>
    {
        public string Action => "setAddressCustomField";
        [Required]
        public string AddressId { get; set; }
        [Required]
        public string Name { get; set; }
        public object Value { get; set; }
    }
}