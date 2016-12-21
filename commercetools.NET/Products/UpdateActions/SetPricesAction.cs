using System;
using System.Collections.Generic;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Products.UpdateActions
{
    /// <summary>
    /// Sets the prices of a product variant.
    /// </summary>
    /// <remarks>
    /// The same validation rules as for addPrice apply. All previous price information is lost and even if some prices did not change, all the prices will have new ids.
    /// </remarks>
    /// <see href="http://dev.commercetools.com/http-api-projects-products.html#set-prices"/>
    public class SetPricesAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Variant ID
        /// </summary>
        [JsonProperty(PropertyName = "variantId")]
        public int? VariantId { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        [JsonProperty(PropertyName = "sku")]
        public string Sku { get; set; }

        /// <summary>
        /// Prices
        /// </summary>
        [JsonProperty(PropertyName = "prices")]
        public List<PriceDraft> Prices { get; set; }

        /// <summary>
        /// Staged
        /// </summary>
        [JsonProperty(PropertyName = "staged")]
        public bool Staged { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <remarks>
        /// Either variantId or sku must be specified.
        /// </remarks>
        /// <param name="prices">Prices</param>
        /// <param name="variantId">Product variant ID</param>
        /// <param name="sku">Product variant SKU</param>
        public SetPricesAction(List<PriceDraft> prices, int? variantId = null, string sku = null)
        {
            if (!variantId.HasValue && string.IsNullOrWhiteSpace(sku))
            {
                throw new ArgumentException("Either variantId or sku are required");
            }

            this.Action = "setPrices";
            this.Prices = prices;
            this.VariantId = variantId;
            this.Sku = sku;
        }

        #endregion
    }
}
