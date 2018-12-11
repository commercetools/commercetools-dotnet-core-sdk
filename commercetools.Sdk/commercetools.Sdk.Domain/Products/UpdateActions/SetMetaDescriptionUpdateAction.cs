namespace commercetools.Sdk.Domain.Products
{
    public class SetMetaDescriptionUpdateAction : UpdateAction<Product>
    {
        public string Action => "setMetaDescription";
        public LocalizedString MetaDescription { get; set; }
        public bool Staged { get; set; }
    }
}