using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Types.UpdateActions
{
    public class AddEnumToFieldDefinitionUpdateAction : UpdateAction<Type>
    {
        public string Action => "addEnumValue";
        [Required]
        public string FieldName { get; set; }
        [Required]
        public EnumValue Value { get; set; }
    }
}
