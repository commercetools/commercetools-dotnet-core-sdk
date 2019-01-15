namespace commercetools.Sdk.Domain.Categories.UpdateActions
{
    public class SetMetaDescriptionUpdateAction : UpdateAction<Category>
    {
        public string Action => "setMetaDescription";
        public LocalizedString MetaDescription { get; set; }
    }
}