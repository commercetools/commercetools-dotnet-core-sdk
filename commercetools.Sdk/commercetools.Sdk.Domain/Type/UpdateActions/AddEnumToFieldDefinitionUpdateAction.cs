namespace commercetools.Sdk.Domain
{
    public class AddEnumToFieldDefinitionUpdateAction : UpdateAction<Type>
    {
        public string Action => "addEnumValue";
        public string FieldName { get; set; }
        public EnumValue Value { get; set; }
    }
}