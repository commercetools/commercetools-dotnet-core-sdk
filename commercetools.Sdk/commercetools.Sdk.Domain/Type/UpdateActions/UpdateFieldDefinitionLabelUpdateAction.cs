using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain
{
    public class ChangeFieldDefinitionLabelUpdateAction : UpdateAction<Type>
    {
        public string Action => "changeLabel";
        [Required]
        public string FieldName { get; set; }
        [Required]
        public LocalizedString Label { get; set; }
    }
}
