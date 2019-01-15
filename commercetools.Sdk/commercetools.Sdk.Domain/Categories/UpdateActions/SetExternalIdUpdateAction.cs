namespace commercetools.Sdk.Domain.Categories.UpdateActions
{
    public class SetExternalIdUpdateAction : UpdateAction<Category>
    {
        public string Action => "setExternalId";
        public string ExternalId { get; set; }
    }
}