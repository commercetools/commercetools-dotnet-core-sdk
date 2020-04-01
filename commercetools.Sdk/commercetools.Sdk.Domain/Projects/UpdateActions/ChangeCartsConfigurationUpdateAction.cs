namespace commercetools.Sdk.Domain.Projects.UpdateActions
{
    public class ChangeCartsConfigurationUpdateAction : UpdateAction<Project>
    {
        public string Action => "changeCartsConfiguration";
        public CartsConfiguration CartsConfiguration { get; set; }
    }
}
