namespace commercetools.Sdk.Domain.Products
{
    public class SetProductVariantKeyUpdateAction : UpdateAction<Product>
    {
        public string Action => "setProductVariantKey";
        public int? VariantId { get; set; }
        public string Sku { get; set; }
        public string Key { get; set; }
        public bool Staged { get; set; }

        private SetProductVariantKeyUpdateAction(string key, bool staged = true)
        {
            this.Key = key;
            this.Staged = staged;
        }

        public SetProductVariantKeyUpdateAction(string sku, string key, bool staged = true) : this(key,staged)
        {
            this.Sku = sku;
        }
        public SetProductVariantKeyUpdateAction(int variantId, string key, bool staged = true) : this(key,staged)
        {
            this.VariantId = variantId;
        }
    }
}
