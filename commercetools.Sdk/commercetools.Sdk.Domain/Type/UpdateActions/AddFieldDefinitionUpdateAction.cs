namespace commercetools.Sdk.Domain
{
    public class AddFieldDefinitionUpdateAction : UpdateAction<Type>
    {
        public string Action => "addFieldDefinition";
        public FieldDefinition FieldDefinition { get; set; }
    }
}