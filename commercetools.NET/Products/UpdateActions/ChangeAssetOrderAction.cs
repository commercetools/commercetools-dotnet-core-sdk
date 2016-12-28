using System;
using System.Collections.Generic;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Products.UpdateActions
{
    /// <summary>
    /// Changes the order of the assets array. 
    /// </summary>
    /// <remarks>
    /// The new order is defined by listing the ids of the assets.
    /// </remarks>
    /// <see href="http://dev.commercetools.com/http-api-projects-products.html#change-asset-order"/>
    public class ChangeAssetOrderAction : UpdateAction
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
        /// Must contain all asset ids.
        /// </summary>
        [JsonProperty(PropertyName = "assetOrder")]
        public List<string> AssetOrder { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="assetOrder">Must contain all asset ids.</param>
        /// <param name="variantId">Variant ID</param>
        /// <param name="sku">Sku</param>
        public ChangeAssetOrderAction(List<string> assetOrder, int? variantId = null, string sku = null)
        {
            if (!variantId.HasValue && string.IsNullOrWhiteSpace(sku))
            {
                throw new ArgumentException("Either variantId or sku are required");
            }

            this.Action = "changeAssetOrder";
            this.AssetOrder = assetOrder;
            this.VariantId = variantId;
            this.Sku = sku;
        }

        #endregion
    }
}
