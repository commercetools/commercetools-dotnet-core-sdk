namespace commercetools.Sdk.Domain.Products
{
    public class SetProductVariantKeyUpdateAction : UpdateAction<Product>
    {
        public string Action => "setProductVariantKey";
        public int VariantId { get; set; }
        public string Sku { get; set; }
        public string Key { get; set; }
        public bool Staged { get; set; }
    }
}