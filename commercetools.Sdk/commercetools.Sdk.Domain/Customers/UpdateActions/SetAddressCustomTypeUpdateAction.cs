using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Customers.UpdateActions
{
    public class SetAddressCustomTypeUpdateAction : UpdateAction<Customer>
    {
        public string Action => "setAddressCustomType";
        [Required] 
        public string AddressId { get; set; }
        public ResourceIdentifier Type { get; set; }
        public Fields Fields { get; set; }
    }
}