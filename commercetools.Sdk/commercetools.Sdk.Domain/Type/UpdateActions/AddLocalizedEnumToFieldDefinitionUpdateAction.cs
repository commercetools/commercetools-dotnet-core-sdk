namespace commercetools.Sdk.Domain
{
    public class AddLocalizedEnumToFieldDefinitionUpdateAction : UpdateAction<Type>
    {
        public string Action => "addLocalizedEnumValue";
        public string FieldName { get; set; }
        public LocalizedEnumValue Value { get; set; }
    }
}