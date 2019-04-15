namespace commercetools.Sdk.Domain.Products.UpdateActions
{
    public class AddAssetUpdateAction : UpdateAction<Product>
    {
        public string Action => "addAsset";
        public int? VariantId { get; set; }
        public string Sku { get; set; }
        public int Position { get; set; }
        public bool Staged { get; set; }
        public AssetDraft Asset { get; set; }

        private AddAssetUpdateAction(AssetDraft asset, int position, bool staged = true)
        {
            this.Asset = asset;
            this.Position = position;
            this.Staged = staged;
        }

        public AddAssetUpdateAction(string sku, AssetDraft asset, int position, bool staged = true) : this(asset,
            position, staged)
        {
            this.Sku = sku;
        }

        public AddAssetUpdateAction(int variantId, AssetDraft asset, int position, bool staged = true) : this(asset,
            position, staged)
        {
            this.VariantId = variantId;
        }
    }
}