using System;

namespace commercetools.Sdk.Domain.Products.UpdateActions
{
    public class ChangeAssetNameUpdateAction : UpdateAction<Product>
    {
        public string Action => "changeAssetName";
        public int? VariantId { get; set; }
        public string Sku { get; set; }
        public bool Staged { get; set; }
        public string AssetId { get; set; }
        public string AssetKey { get; set; }
        public LocalizedString Name { get; set; }


        /// <summary>
        /// Change Asset Name, pass either assetId or assetKey
        /// </summary>
        /// <param name="sku">Sku of the product Variant</param>
        /// <param name="name">new asset name</param>
        /// <param name="assetId">Asset ID</param>
        /// <param name="assetKey">Asset Key</param>
        /// <param name="staged">Staged flag</param>
        public ChangeAssetNameUpdateAction(string sku, LocalizedString name, string assetId = null, string assetKey = null, bool staged = true)
        {
            Init(name, sku, null, assetId, assetKey, staged);
        }

        /// <summary>
        /// Change Asset Name, pass either assetId or assetKey
        /// </summary>
        /// <param name="variantId">Id of the product Variant</param>
        /// <param name="name">new asset name</param>
        /// <param name="assetId">Asset ID</param>
        /// <param name="assetKey">Asset Key</param>
        /// <param name="staged">Staged flag</param>
        public ChangeAssetNameUpdateAction(int variantId, LocalizedString name, string assetId = null, string assetKey = null, bool staged = true)
        {
            Init(name, null, variantId, assetId, assetKey, staged);
        }

        private void Init(LocalizedString name,string sku = null, int? variantId = null, string assetId = null, string assetKey = null,
            bool staged = true)
        {
            this.Name = name;
            this.Sku = sku;
            this.VariantId = variantId;
            this.AssetId = assetId;
            this.AssetKey = assetKey;
            this.Staged = staged;

            // check if both assetId and assetKey are null
            if (string.IsNullOrEmpty(assetId) && string.IsNullOrEmpty(assetKey))
            {
                throw new ArgumentException("Pass either assetId or assetKey parameters");
            }
        }
    }
}
