namespace commercetools.Sdk.Domain.Products.UpdateActions
{
    public class SetSkuUpdateAction : UpdateAction<Product>
    {
        public string Action => "setSku";
        public int VariantId { get; set; }
        public string Sku { get; set; }
        public bool Staged { get; set; }
    }
}