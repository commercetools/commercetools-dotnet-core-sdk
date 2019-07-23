using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain
{
    public class ChangeEnumValueLabelUpdateAction : UpdateAction<Type>
    {
        public string Action => "changeEnumValueLabel";
        [Required]
        public string FieldName { get; set; }
        [Required]
        public EnumValue Value { get; set; }
    }
}
