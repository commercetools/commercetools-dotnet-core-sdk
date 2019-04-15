using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Products
{
    public class SetAssetDescriptionUpdateAction : UpdateAction<Product>
    {
        public string Action => "setAssetDescription";
        public int? VariantId { get; set; }
        public string Sku { get; set; }
        public bool Staged { get; set; }
        public string AssetId { get; set; }
        public string AssetKey { get; set; }
        public LocalizedString Description { get; set; }

        /// <summary>
        /// Set Asset Description, pass either assetId or assetKey
        /// </summary>
        /// <param name="sku">Sku of the product Variant</param>
        /// <param name="description">new asset name</param>
        /// <param name="assetId">Asset ID</param>
        /// <param name="assetKey">Asset Key</param>
        /// <param name="staged">Staged flag</param>
        public SetAssetDescriptionUpdateAction(string sku, LocalizedString description, string assetId = null, string assetKey = null, bool staged = true)
        {
            Init(description, sku, null, assetId, assetKey, staged);
        }

        /// <summary>
        /// Set Asset Description, pass either assetId or assetKey
        /// </summary>
        /// <param name="variantId">Id of the product Variant</param>
        /// <param name="description">new asset name</param>
        /// <param name="assetId">Asset ID</param>
        /// <param name="assetKey">Asset Key</param>
        /// <param name="staged">Staged flag</param>
        public SetAssetDescriptionUpdateAction(int variantId, LocalizedString description, string assetId = null, string assetKey = null, bool staged = true)
        {
            Init(description, null, variantId, assetId, assetKey, staged);
        }

        private void Init(LocalizedString description,string sku = null, int? variantId = null, string assetId = null, string assetKey = null,
            bool staged = true)
        {
            this.Description = description;
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
