namespace commercetools.Sdk.Domain.Zones.UpdateActions
{
    public class SetKeyUpdateAction : UpdateAction<Zone>
    {
        public string Action => "setKey";
        public string Key { get; set; }
    }
}