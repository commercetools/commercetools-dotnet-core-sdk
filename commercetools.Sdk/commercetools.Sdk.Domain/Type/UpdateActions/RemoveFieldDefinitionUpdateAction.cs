using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain
{
    public class RemoveFieldDefinitionUpdateAction : UpdateAction<Type>
    {
        public string Action => "removeFieldDefinition";
        [Required]
        public string FieldName { get; set; }
    }
}
