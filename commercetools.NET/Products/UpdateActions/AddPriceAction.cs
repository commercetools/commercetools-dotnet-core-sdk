using System;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Products.UpdateActions
{
    /// <summary>
    /// Adds the given price to the product variant's prices set.
    /// </summary>
    /// <remarks>
    /// Prices are defined with a scope (currency, country, CustomerGroup and channel) and/or a validity period (validFrom and/or validTo). A price without validity period (no validFrom and no validUntil) is always valid for its scope. 
    /// </remarks>
    /// <see href="http://dev.commercetools.com/http-api-projects-products.html#add-price"/>
    public class AddPriceAction : UpdateAction
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
        /// Price
        /// </summary>
        [JsonProperty(PropertyName = "price")]
        public PriceDraft Price { get; set; }

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
        /// <param name="price">Price</param>
        /// <param name="variantId">Product variant ID</param>
        /// <param name="sku">Product variant SKU</param>
        public AddPriceAction(PriceDraft price, int? variantId = null, string sku = null)
        {
            if (!variantId.HasValue && string.IsNullOrWhiteSpace(sku))
            {
                throw new ArgumentException("Either variantId or sku are required");
            }

            this.Action = "addPrice";
            this.VariantId = variantId;
            this.Sku = sku;
            this.Price = price;
        }

        #endregion
    }
}
