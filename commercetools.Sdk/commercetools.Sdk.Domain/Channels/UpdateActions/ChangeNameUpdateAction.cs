namespace commercetools.Sdk.Domain.Channels.UpdateActions
{
    public class ChangeNameUpdateAction : UpdateAction<Channel>
    {
        public string Action => "changeName";
        public LocalizedString Name { get; set; }
    }
}
