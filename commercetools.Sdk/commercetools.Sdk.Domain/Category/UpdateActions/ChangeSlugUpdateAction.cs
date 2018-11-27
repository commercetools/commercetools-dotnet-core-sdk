namespace commercetools.Sdk.Domain.Categories
{
    public class ChangeSlugUpdateAction : UpdateAction<Category>
    {
        public string Action => "changeSlug";
        public LocalizedString Slug { get; set; }
    }
}