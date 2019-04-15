using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Products
{
    public class RemoveAssetUpdateAction : UpdateAction<Product>
    {
        public string Action => "removeAsset";
        public int? VariantId { get; set; }
        public string Sku { get; set; }
        public bool Staged { get; set; }
        public string AssetId { get; set; }
        public string AssetKey { get; set; }


        /// <summary>
        /// Remove Asset, pass either assetId or assetKey
        /// </summary>
        /// <param name="sku">Sku of the product Variant</param>
        /// <param name="assetId">Asset ID</param>
        /// <param name="assetKey">Asset Key</param>
        /// <param name="staged">Staged flag</param>
        public RemoveAssetUpdateAction(string sku, string assetId = null, string assetKey = null, bool staged = true)
        {
            Init(sku, null, assetId, assetKey, staged);
        }

        /// <summary>
        /// Remove Asset, pass either assetId or assetKey
        /// </summary>
        /// <param name="variantId">Id of the product Variant</param>
        /// <param name="assetId">Asset ID</param>
        /// <param name="assetKey">Asset Key</param>
        /// <param name="staged">Staged flag</param>
        public RemoveAssetUpdateAction(int variantId, string assetId, string assetKey, bool staged = true)
        {
            Init(null, variantId, assetId, assetKey, staged);
        }

        private void Init(string sku = null, int? variantId = null, string assetId = null, string assetKey = null,
            bool staged = true)
        {
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
