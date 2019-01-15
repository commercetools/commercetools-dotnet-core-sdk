namespace commercetools.Sdk.Domain.Categories.UpdateActions
{
    public class SetMetaKeywordsUpdateAction : UpdateAction<Category>
    {
        public string Action => "setMetaKeywords";
        public LocalizedString MetaKeywords { get; set; }
    }
}