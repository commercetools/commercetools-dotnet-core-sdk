namespace commercetools.Sdk.Domain.Categories
{
    public class SetMetaTitleUpdateAction : UpdateAction<Category>
    {
        public string Action => "setMetaTitle";
        public LocalizedString MetaTitle { get; set; }
    }
}