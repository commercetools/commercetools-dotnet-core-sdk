namespace commercetools.Sdk.Domain.Products.UpdateActions
{
    public class SetMetaKeywordsUpdateAction : UpdateAction<Product>
    {
        public string Action => "setMetaKeywords";
        public LocalizedString MetaKeywords { get; set; }
        public bool Staged { get; set; }
    }
}