namespace commercetools.Sdk.Domain.Projects.UpdateActions
{
    public class ChangeProductSearchIndexingEnabledUpdateAction : UpdateAction<Project>
    {
        public string Action => "changeProductSearchIndexingEnabled";
        public bool Enabled { get; set; }
    }
}