using System;
using System.Collections.Generic;

namespace commercetools.Sdk.Domain.Products.UpdateActions
{
    public class SetAssetSourcesUpdateAction : UpdateAction<Product>
    {
        public string Action => "setAssetSources";
        public int? VariantId { get; set; }
        public string Sku { get; set; }
        public bool Staged { get; set; }
        public string AssetId { get; set; }
        public string AssetKey { get; set; }
        public List<AssetSource> Sources { get; set; }

        /// <summary>
        /// Set Asset Sources, pass either assetId or assetKey
        /// </summary>
        /// <param name="sku">Sku of the product Variant</param>
        /// <param name="sources">asset sources</param>
        /// <param name="assetId">Asset ID</param>
        /// <param name="assetKey">Asset Key</param>
        /// <param name="staged">Staged flag</param>
        public SetAssetSourcesUpdateAction(string sku, List<AssetSource> sources, string assetId = null, string assetKey = null, bool staged = true)
        {
            Init(sources, sku, null, assetId, assetKey, staged);
        }

        /// <summary>
        /// Set Asset Sources, pass either assetId or assetKey
        /// </summary>
        /// <param name="variantId">Id of the product Variant</param>
        /// <param name="sources">asset sources</param>
        /// <param name="assetId">Asset ID</param>
        /// <param name="assetKey">Asset Key</param>
        /// <param name="staged">Staged flag</param>
        public SetAssetSourcesUpdateAction(int variantId, List<AssetSource> sources, string assetId = null, string assetKey = null, bool staged = true)
        {
            Init(sources, null, variantId, assetId, assetKey, staged);
        }

        private void Init(List<AssetSource> sources,string sku = null, int? variantId = null, string assetId = null, string assetKey = null,
            bool staged = true)
        {
            this.Sources = sources;
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
