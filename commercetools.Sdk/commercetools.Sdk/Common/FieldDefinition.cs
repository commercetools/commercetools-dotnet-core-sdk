namespace commercetools.Sdk.Domain
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