namespace commercetools.Sdk.Domain.Categories
{
    public class SetDescriptionUpdateAction : UpdateAction<Category>
    {
        public string Action => "setDescription";
        public string Description { get; set; }
    }
}