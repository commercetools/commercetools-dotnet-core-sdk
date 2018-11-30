namespace commercetools.Sdk.Domain
{
    public class AttributeDefinitionDraft
    {
        public AttributeType Type { get; set; }
        [AttributeName]
        public string Name { get; set; }
        public LocalizedString Label { get; set; }
        public bool IsRequired { get; set; }
        public AttributeConstraint AttributeConstraint { get; set; }
        public LocalizedString InputTip { get; set; }
        public TextInputHint InputHint { get; set; }
        public bool IsSearchable { get; set; }
    }
}