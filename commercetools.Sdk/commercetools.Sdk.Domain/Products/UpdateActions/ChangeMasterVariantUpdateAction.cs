namespace commercetools.Sdk.Domain.Products.UpdateActions
{
    public class ChangeMasterVariantUpdateAction : UpdateAction<Product>
    {
        public string Action => "changeMasterVariant";
        public int? VariantId { get; set; }
        public string Sku { get; set; }
        public bool Staged { get; set; }

        private ChangeMasterVariantUpdateAction(bool staged = true)
        {
            this.Staged = staged;
        }

        public ChangeMasterVariantUpdateAction(string sku, bool staged = true) : this(staged)
        {
            this.Sku = sku;
        }
        public ChangeMasterVariantUpdateAction(int variantId, bool staged = true) : this(staged)
        {
            this.VariantId = variantId;
        }
    }
}
