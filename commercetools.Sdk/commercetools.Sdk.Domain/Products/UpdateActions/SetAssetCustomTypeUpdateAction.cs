using System;
using System.Collections.Generic;

namespace commercetools.Sdk.Domain.Products
{
    using System.ComponentModel.DataAnnotations;

    public class SetAssetCustomTypeUpdateAction : UpdateAction<Product>
    {
        public string Action => "setAssetCustomType";
        public int? VariantId { get; set; }
        public string Sku { get; set; }
        public bool Staged { get; set; }
        public string AssetId { get; set; }
        public string AssetKey { get; set; }
        public ResourceIdentifier<Type> Type { get; set; }
        public Fields Fields { get; set; }

        /// <summary>
        /// Set Asset Custom Type, pass either assetId or assetKey
        /// </summary>
        /// <param name="sku">Sku of the product Variant</param>
        /// <param name="type">asset custom type</param>
        /// <param name="fields">asset custom fields</param>
        /// <param name="assetId">Asset ID</param>
        /// <param name="assetKey">Asset Key</param>
        /// <param name="staged">Staged flag</param>
        public SetAssetCustomTypeUpdateAction(string sku, ResourceIdentifier<Type> type = null, Fields fields = null, string assetId = null, string assetKey = null, bool staged = true)
        {
            Init(type, fields, sku, null, assetId, assetKey, staged);
        }

        /// <summary>
        /// Set Asset Custom Type, pass either assetId or assetKey
        /// </summary>
        /// <param name="variantId">Id of the product Variant</param>
        /// <param name="type">asset custom type</param>
        /// <param name="fields">asset custom fields</param>
        /// <param name="assetId">Asset ID</param>
        /// <param name="assetKey">Asset Key</param>
        /// <param name="staged">Staged flag</param>
        public SetAssetCustomTypeUpdateAction(int variantId, ResourceIdentifier<Type> type = null, Fields fields = null, string assetId = null, string assetKey = null, bool staged = true)
        {
            Init(type, fields, null, variantId, assetId, assetKey, staged);
        }

        private void Init(ResourceIdentifier<Type> type = null, Fields fields = null,string sku = null, int? variantId = null, string assetId = null, string assetKey = null,
            bool staged = true)
        {
            this.Type = type;
            this.Fields = fields;
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
