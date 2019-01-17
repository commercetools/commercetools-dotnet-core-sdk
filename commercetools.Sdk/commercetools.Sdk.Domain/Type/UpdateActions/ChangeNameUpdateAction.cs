namespace commercetools.Sdk.Domain.Types.UpdateActions
{
    public class ChangeNameUpdateAction : UpdateAction<Type>
    {
        public string Action => "changeName";
        public LocalizedString Name { get; set; }
    }
}