using commercetools.Sdk.Domain.Types.FieldTypes;

namespace commercetools.Sdk.Domain.Types
{
    public class FieldDefinition
    {
        public FieldType Type { get; set; }
        public string Name { get; set; }
        public LocalizedString Label { get; set; }
        public bool Required { get; set; }
        public TextInputHint InputHint { get; set; }
    }
}