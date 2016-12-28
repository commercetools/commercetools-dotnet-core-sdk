using Newtonsoft.Json;

namespace commercetools.Products
{
    /// <summary>
    /// ProductVariantAvailability
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-products.html#productvariantavailability"/>
    public class ProductVariantAvailability
    {
        #region Properties

        [JsonProperty(PropertyName = "isOnStock")]
        public bool? IsOnStock { get; private set; }

        [JsonProperty(PropertyName = "restockableInDays")]
        public int? RestockableInDays { get; private set; }

        [JsonProperty(PropertyName = "availableQuantity")]
        public int? AvailableQuantity { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public ProductVariantAvailability(dynamic data = null)
        {
            if (data == null)
            {
                return;
            }

            this.IsOnStock = data.isOnStock;
            this.RestockableInDays = data.restockableInDays;
            this.AvailableQuantity = data.availableQuantity;
        }

        #endregion
    }
}
