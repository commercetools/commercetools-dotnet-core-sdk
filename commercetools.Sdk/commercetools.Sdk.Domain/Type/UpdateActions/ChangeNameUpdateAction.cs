namespace commercetools.Sdk.Domain
{
    public class ChangeNameUpdateAction : UpdateAction<Type>
    {
        public string Action => "changeName";
        public LocalizedString Name { get; set; }
    }
}