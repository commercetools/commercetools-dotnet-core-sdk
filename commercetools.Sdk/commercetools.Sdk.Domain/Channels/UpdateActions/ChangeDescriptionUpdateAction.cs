namespace commercetools.Sdk.Domain.Channels.UpdateActions
{
    public class ChangeDescriptionUpdateAction : UpdateAction<Channel>
    {
        public string Action => "changeDescription";
        public LocalizedString Description { get; set; }
    }
}
