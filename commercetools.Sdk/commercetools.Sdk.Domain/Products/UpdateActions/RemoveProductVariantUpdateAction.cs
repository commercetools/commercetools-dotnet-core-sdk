namespace commercetools.Sdk.Domain.Products.UpdateActions
{
    public class RemoveProductVariantUpdateAction : UpdateAction<Product>
    {
        public string Action => "removeVariant";
        public int Id { get; set; }
        public string Sku { get; set; }        
        public bool Staged { get; set; }
    }
}