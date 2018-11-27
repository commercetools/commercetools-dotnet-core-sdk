namespace commercetools.Sdk.Domain.Categories
{
    public class SetExternalIdUpdateAction : UpdateAction<Category>
    {
        public string Action => "setExternalId";
        public string ExternalId { get; set; }
    }
}