namespace commercetools.Sdk.Domain.APIExtensions.UpdateActions
{
    public class SetKeyUpdateAction : UpdateAction<Extension>
    {
        public string Action => "setKey";
        public string Key { get; set; }
    }
}
