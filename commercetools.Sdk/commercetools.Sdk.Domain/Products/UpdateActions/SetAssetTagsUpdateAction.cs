using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Products
{
    public class SetAssetTagsUpdateAction : UpdateAction<Product>
    {
        public string Action => "setAssetTags";
        public int? VariantId { get; set; }
        public string Sku { get; set; }
        public bool Staged { get; set; }
        public string AssetId { get; set; }
        public string AssetKey { get; set; }
        public List<string> Tags { get; set; }

        /// <summary>
        /// Set Asset Tags, pass either assetId or assetKey
        /// </summary>
        /// <param name="sku">Sku of the product Variant</param>
        /// <param name="tags">asset tags</param>
        /// <param name="assetId">Asset ID</param>
        /// <param name="assetKey">Asset Key</param>
        /// <param name="staged">Staged flag</param>
        public SetAssetTagsUpdateAction(string sku, List<string> tags, string assetId = null, string assetKey = null, bool staged = true)
        {
            Init(tags, sku, null, assetId, assetKey, staged);
        }

        /// <summary>
        /// Set Asset Tags, pass either assetId or assetKey
        /// </summary>
        /// <param name="variantId">Id of the product Variant</param>
        /// <param name="tags">asset tags</param>
        /// <param name="assetId">Asset ID</param>
        /// <param name="assetKey">Asset Key</param>
        /// <param name="staged">Staged flag</param>
        public SetAssetTagsUpdateAction(int variantId, List<string> tags, string assetId = null, string assetKey = null, bool staged = true)
        {
            Init(tags, null, variantId, assetId, assetKey, staged);
        }

        private void Init(List<string> tags,string sku = null, int? variantId = null, string assetId = null, string assetKey = null,
            bool staged = true)
        {
            this.Tags = tags;
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
