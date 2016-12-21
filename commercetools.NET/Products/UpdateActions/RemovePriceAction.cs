using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Products.UpdateActions
{
    /// <summary>
    /// Removes a price.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-products.html#remove-price"/>
    public class RemovePriceAction : UpdateAction
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

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="priceId">Price ID</param>
        public RemovePriceAction(string priceId)
        {
            this.Action = "removePrice";
            this.PriceId = priceId;
        }

        #endregion
    }
}
