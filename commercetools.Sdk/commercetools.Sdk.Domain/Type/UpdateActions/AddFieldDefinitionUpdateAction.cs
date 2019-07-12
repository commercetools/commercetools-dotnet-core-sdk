using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain
{
    public class AddFieldDefinitionUpdateAction : UpdateAction<Type>
    {
        public string Action => "addFieldDefinition";
        [Required]
        public FieldDefinition FieldDefinition { get; set; }
    }
}
