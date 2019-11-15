using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Types.UpdateActions
{
    public class ChangeInputHintUpdateAction : UpdateAction<Type>
    {
        public string Action => "changeInputHint";

        [Required]
        public string FieldName { get; set; }

        [Required]
        public TextInputHint InputHint { get; set; }
    }
}
