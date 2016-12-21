using System;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Products.UpdateActions
{
    /// <summary>
    /// Change Asset Name
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-products.html#change-asset-name"/>
    public class ChangeAssetNameAction : UpdateAction
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
        /// Asset ID
        /// </summary>
        [JsonProperty(PropertyName = "assetId")]
        public string AssetId { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public LocalizedString Name { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="assetId">Asset ID</param>
        /// <param name="name">Name</param>
        /// <param name="variantId">Variant ID</param>
        /// <param name="sku">Sku</param>
        public ChangeAssetNameAction(string assetId, LocalizedString name, int? variantId = null, string sku = null)
        {
            if (!variantId.HasValue && string.IsNullOrWhiteSpace(sku))
            {
                throw new ArgumentException("Either variantId or sku are required");
            }

            this.Action = "changeAssetName";
            this.AssetId = assetId;
            this.Name = name;
            this.VariantId = variantId;
            this.Sku = sku;
        }

        #endregion
    }
}
