using System;

namespace commercetools.Sdk.Domain.Products.UpdateActions
{
    public class SetAssetCustomFieldUpdateAction : UpdateAction<Product>
    {
        public string Action => "setAssetCustomField";
        public int? VariantId { get; set; }
        public string Sku { get; set; }
        public bool Staged { get; set; }
        public string AssetId { get; set; }
        public string AssetKey { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }

        /// <summary>
        /// Set Asset Custom Field, pass either assetId or assetKey
        /// </summary>
        /// <param name="sku">Sku of the product Variant</param>
        /// <param name="name">asset custom field name</param>
        /// <param name="value">asset custom field value</param>
        /// <param name="assetId">Asset ID</param>
        /// <param name="assetKey">Asset Key</param>
        /// <param name="staged">Staged flag</param>
        public SetAssetCustomFieldUpdateAction(string sku, string name, Object value = null, string assetId = null, string assetKey = null, bool staged = true)
        {
            Init(name, value, sku, null, assetId, assetKey, staged);
        }

        /// <summary>
        /// Set Asset Custom Field, pass either assetId or assetKey
        /// </summary>
        /// <param name="variantId">Id of the product Variant</param>
        /// <param name="name">asset custom field name</param>
        /// <param name="value">asset custom field value</param>
        /// <param name="assetId">Asset ID</param>
        /// <param name="assetKey">Asset Key</param>
        /// <param name="staged">Staged flag</param>
        public SetAssetCustomFieldUpdateAction(int variantId, string name, Object value = null, string assetId = null, string assetKey = null, bool staged = true)
        {
            Init(name, value, null, variantId, assetId, assetKey, staged);
        }

        private void Init(string name, object value = null,string sku = null, int? variantId = null, string assetId = null, string assetKey = null,
            bool staged = true)
        {
            this.Name = name;
            this.Value = value;
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
