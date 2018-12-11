namespace commercetools.Sdk.Domain.Products
{
    using System.ComponentModel.DataAnnotations;

    public class SetAssetCustomTypeUpdateAction : UpdateAction<Product>
    {
        public string Action => "setAssetCustomType";        
        public int VariantId { get; set; }
        public string Sku { get; set; }
        public bool Staged { get; set; }
        public string AssetId { get; set; }
        public string AssetKey { get; set; }
        public ResourceIdentifier Type { get; set; }
        public Fields Fields { get; set; }
    }
}