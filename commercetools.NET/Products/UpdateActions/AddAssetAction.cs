using System;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Products.UpdateActions
{
    /// <summary>
    /// Adds an Asset.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-products.html#add-asset"/>
    public class AddAssetAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Variant ID
        /// </summary>
        [JsonProperty(PropertyName = "variantId")]
        public int? VariantId { get; set; }

        /// <summary>
        /// Sku
        /// </summary>
        [JsonProperty(PropertyName = "sku")]
        public string Sku { get; set; }

        /// <summary>
        /// Staged
        /// </summary>
        /// <remarks>
        /// Defaults to true
        /// </remarks>
        [JsonProperty(PropertyName = "staged")]
        public bool Staged { get; set; }

        /// <summary>
        /// Asset
        /// </summary>
        [JsonProperty(PropertyName = "asset")]
        public AssetDraft Asset { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="asset">Asset</param>
        /// <param name="variantId">Variant ID</param>
        /// <param name="sku">Sku</param>
        public AddAssetAction(AssetDraft asset, int? variantId = null, string sku = null)
        {
            if (!variantId.HasValue && string.IsNullOrWhiteSpace(sku))
            {
                throw new ArgumentException("Either variantId or sku are required");
            }

            this.Action = "addAsset";
            this.Asset = asset;
            this.VariantId = variantId;
            this.Sku = sku;
        }

        #endregion
    }
}
