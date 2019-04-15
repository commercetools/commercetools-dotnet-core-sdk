using System;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Products.UpdateActions
{
    public class SetAssetKeyUpdateAction : UpdateAction<Product>
    {
        public string Action => "setAssetKey";
        public int? VariantId { get; set; }
        public string Sku { get; set; }
        public bool Staged { get; set; }
        [Required]
        public string AssetId { get; set; }
        public string AssetKey { get; set; }


        public SetAssetKeyUpdateAction(string sku, string assetId, string assetKey = null, bool staged = true)
        {
            Init(sku, null, assetId, assetKey, staged);
        }
        public SetAssetKeyUpdateAction(int variantId, string assetId, string assetKey = null, bool staged = true)
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

            // check is assetId is empty or null
            if (string.IsNullOrEmpty(assetId))
            {
                throw new ArgumentException("assetId parameter is required");
            }
        }
    }
}
