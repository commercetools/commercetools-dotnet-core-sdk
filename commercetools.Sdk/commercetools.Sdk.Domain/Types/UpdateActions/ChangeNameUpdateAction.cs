using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Types.UpdateActions
{
    public class ChangeNameUpdateAction : UpdateAction<Type>
    {
        public string Action => "changeName";
        [Required]
        public LocalizedString Name { get; set; }
    }
}
