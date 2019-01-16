using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.CustomerGroups.UpdateActions
{
    public class ChangeNameUpdateAction : UpdateAction<CustomerGroup>
    {
        public string Action => "changeName";
        [Required]
        public string Name { get; set; }
    }
}
