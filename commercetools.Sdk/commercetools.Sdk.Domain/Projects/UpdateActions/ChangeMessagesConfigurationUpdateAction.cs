namespace commercetools.Sdk.Domain.Projects.UpdateActions
{
    public class ChangeMessagesConfigurationUpdateAction : UpdateAction<Project>
    {
        public string Action => "changeMessagesConfiguration";
        public MessagesConfigurationDraft MessagesConfiguration { get; set; }
    }
}
