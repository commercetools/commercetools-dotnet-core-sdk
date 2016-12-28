using System;
using System.Collections.Generic;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Products.UpdateActions
{
    /// <summary>
    /// Set Asset Sources
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-products.html#set-asset-sources"/>
    public class SetAssetSourcesAction : UpdateAction
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
        /// List of AssetSource
        /// </summary>
        /// <remarks>
        /// Requires at least one entry
        /// </remarks>
        [JsonProperty(PropertyName = "sources")]
        public List<AssetSource> Sources { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="assetId">Asset ID</param>
        /// <param name="sources">List of AssetSource</param>
        /// <param name="variantId">Variant ID</param>
        /// <param name="sku">Sku</param>
        public SetAssetSourcesAction(string assetId, List<AssetSource> sources, int? variantId = null, string sku = null)
        {
            if (!variantId.HasValue && string.IsNullOrWhiteSpace(sku))
            {
                throw new ArgumentException("Either variantId or sku are required");
            }

            if (sources == null || sources.Count < 1)
            {
                throw new ArgumentException("sources requires at least one entry");
            }

            this.Action = "setAssetSources";
            this.AssetId = assetId;
            this.Sources = Sources;
            this.VariantId = variantId;
            this.Sku = sku;
        }

        #endregion
    }
}
