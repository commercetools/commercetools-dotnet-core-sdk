namespace commercetools.Sdk.Domain.Categories.UpdateActions
{
    public class SetDescriptionUpdateAction : UpdateAction<Category>
    {
        public string Action => "setDescription";
        public LocalizedString Description { get; set; }
    }
}