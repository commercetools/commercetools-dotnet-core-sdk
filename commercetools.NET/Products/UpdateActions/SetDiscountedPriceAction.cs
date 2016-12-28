using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Products.UpdateActions
{
    /// <summary>
    /// Discounts a product price. 
    /// </summary>
    /// <remarks>
    /// The referenced Product Discount in the discounted field must be active, valid, of type external and it’s predicate must match the referenced price.
    /// </remarks>
    /// <see href="http://dev.commercetools.com/http-api-projects-products.html#set-discounted-price"/>
    public class SetDiscountedPriceAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Price ID
        /// </summary>
        [JsonProperty(PropertyName = "priceId")]
        public string PriceId { get; set; }

        /// <summary>
        /// Staged
        /// </summary>
        [JsonProperty(PropertyName = "staged")]
        public bool Staged { get; set; }

        /// <summary>
        /// Discounted Price
        /// </summary>
        [JsonProperty(PropertyName = "discounted")]
        public DiscountedPrice Discounted { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="priceId">Price ID</param>
        public SetDiscountedPriceAction(string priceId)
        {
            this.Action = "setDiscountedPrice";
            this.PriceId = priceId;
        }

        #endregion
    }
}
