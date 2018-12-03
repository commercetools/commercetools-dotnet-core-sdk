namespace commercetools.Sdk.Domain.Products
{
    public class SetMetaTitleUpdateAction : UpdateAction<Product>
    {
        public string Action => "setMetaTitle";
        public LocalizedString MetaTitle { get; set; }
        public bool Staged { get; set; }
    }
}