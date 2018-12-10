using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Zones.UpdateActions
{
    public class ChangeNameUpdateAction : UpdateAction<Zone>
    {
        public string Action => "changeName";
        [Required]
        public LocalizedString Name { get; set; }
    }
}