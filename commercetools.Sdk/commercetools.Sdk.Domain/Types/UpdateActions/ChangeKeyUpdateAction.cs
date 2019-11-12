using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Types.UpdateActions
{
    public class ChangeKeyUpdateAction : UpdateAction<Type>
    {
        public string Action => "changeKey";
        [Required]
        public string Key { get; set; }
    }
}
