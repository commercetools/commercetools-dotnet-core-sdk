namespace commercetools.Sdk.Domain.Projects.UpdateActions
{
    public class SetExternalOAuthUpdateAction : UpdateAction<Project>
    {
        public string Action => "setExternalOAuth";
        public ExternalOAuth ExternalOAuth { get; set; }
    }
}
