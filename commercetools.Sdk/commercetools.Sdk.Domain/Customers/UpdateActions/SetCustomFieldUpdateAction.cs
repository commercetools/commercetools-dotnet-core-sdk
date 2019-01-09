using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Customers.UpdateActions
{
    public class SetCustomFieldUpdateAction : UpdateAction<Customer>
    {
        public string Action => "setCustomField";
        [Required]
        public string Name { get; set; }
        public object Value { get; set; }
    }
}