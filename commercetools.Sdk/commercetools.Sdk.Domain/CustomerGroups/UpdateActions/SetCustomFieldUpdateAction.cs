using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.CustomerGroups.UpdateActions
{
    public class SetCustomFieldUpdateAction : UpdateAction<CustomerGroup>
    {
        public string Action => "setCustomField";
        [Required]
        public string Name { get; set; }
        public object Value { get; set; }
    }
}