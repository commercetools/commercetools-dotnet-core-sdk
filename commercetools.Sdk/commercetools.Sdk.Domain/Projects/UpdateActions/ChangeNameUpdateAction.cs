namespace commercetools.Sdk.Domain.Projects.UpdateActions
{
    public class ChangeNameUpdateAction : UpdateAction<Project>
    {
        public string Action => "changeName";
        public string Name { get; set; }
    }
}
