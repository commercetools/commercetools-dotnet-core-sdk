namespace commercetools.Sdk.Domain.Products.UpdateActions
{
    public class RemoveProductVariantUpdateAction : UpdateAction<Product>
    {
        public string Action => "removeVariant";
        public int? Id { get; set; }
        public string Sku { get; set; }        
        public bool Staged { get; set; }

        public RemoveProductVariantUpdateAction(string sku, bool staged = true)
        {
            this.Sku = sku;
            this.Staged = staged;
        }
        public RemoveProductVariantUpdateAction(int variantId, bool staged = true)
        {
            this.Id = variantId;
            this.Staged = staged;
        }
    }
}