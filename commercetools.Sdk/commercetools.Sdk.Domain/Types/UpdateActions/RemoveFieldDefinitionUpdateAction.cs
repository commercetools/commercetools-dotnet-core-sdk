using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Types.UpdateActions
{
    public class RemoveFieldDefinitionUpdateAction : UpdateAction<Type>
    {
        public string Action => "removeFieldDefinition";
        [Required]
        public string FieldName { get; set; }
    }
}
