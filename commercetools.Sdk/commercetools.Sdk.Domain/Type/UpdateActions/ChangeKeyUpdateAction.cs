namespace commercetools.Sdk.Domain
{
    public class ChangeKeyUpdateAction : UpdateAction<Type>
    {
        public string Action => "changeKey";
        public string Key { get; set; }
    }
}