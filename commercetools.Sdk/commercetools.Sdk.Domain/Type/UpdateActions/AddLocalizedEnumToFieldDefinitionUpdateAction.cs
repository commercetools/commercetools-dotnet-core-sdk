using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain
{
    public class AddLocalizedEnumToFieldDefinitionUpdateAction : UpdateAction<Type>
    {
        public string Action => "addLocalizedEnumValue";
        [Required]
        public string FieldName { get; set; }
        [Required]
        public LocalizedEnumValue Value { get; set; }
    }
}
