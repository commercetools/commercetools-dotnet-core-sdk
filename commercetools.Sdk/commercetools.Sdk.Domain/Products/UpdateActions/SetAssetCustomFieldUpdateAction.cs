namespace commercetools.Sdk.Domain.Products
{
    using System.ComponentModel.DataAnnotations;

    public class SetAssetCustomFieldUpdateAction : UpdateAction<Product>
    {
        public string Action => "setAssetCustomField";
        public int VariantId { get; set; }
        public string Sku { get; set; }
        public bool Staged { get; set; }
        public string AssetId { get; set; }
        public string AssetKey { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }
    }
}