namespace commercetools.Sdk.Domain.Channels.UpdateActions
{
    public class ChangeKeyUpdateAction : UpdateAction<Channel>
    {
        public string Action => "changeKey";
        public string Key { get; set; }
    }
}
