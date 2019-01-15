namespace commercetools.Sdk.Domain.Categories.UpdateActions
{
    public class SetDescriptionUpdateAction : UpdateAction<Category>
    {
        public string Action => "setDescription";
        public string Description { get; set; }
    }
}