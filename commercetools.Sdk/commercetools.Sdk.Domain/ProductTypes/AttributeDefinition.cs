namespace commercetools.Sdk.Domain
{
    public class AttributeDefinition
    {
        public AttributeType Type { get; set; }
        public string Name { get; set; }
        public LocalizedString Label { get; set; }
        public bool IsRequired { get; set; }
        public AttributeConstraint AttributeConstraint { get; set; }
        public LocalizedString InputTip { get; set; }
        public TextInputHint InputHint { get; set; }
        public bool IsSearchable { get; set; }
    }
}