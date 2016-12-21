using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Products.UpdateActions
{
    /// <summary>
    /// Replaces a price in the product variant's prices set. The price to replace is specified by its ID.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-products.html#change-price"/>
    public class ChangePriceAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Price ID
        /// </summary>
        [JsonProperty(PropertyName = "priceId")]
        public string PriceId { get; set; }

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
        /// <param name="priceId">Price ID</param>
        /// <param name="price">Price</param>
        public ChangePriceAction(string priceId, PriceDraft price)
        {
            this.Action = "changePrice";
            this.PriceId = priceId;
            this.Price = price;
        }

        #endregion
    }
}
