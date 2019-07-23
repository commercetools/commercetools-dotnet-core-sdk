using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain
{
    public class ChangeLocalizedEnumValueLabelUpdateAction : UpdateAction<Type>
    {
        public string Action => "changeLocalizedEnumValueLabel";
        [Required]
        public string FieldName { get; set; }
        [Required]
        public LocalizedEnumValue Value { get; set; }
    }
}
