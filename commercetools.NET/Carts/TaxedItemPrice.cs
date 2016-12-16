using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Carts
{
    /// <summary>
    /// TaxedItemPrice
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#taxeditemprice"/>
    public class TaxedItemPrice
    {
        #region Properties

        [JsonProperty(PropertyName = "totalNet")]
        public Money TotalNet { get; private set; }

        [JsonProperty(PropertyName = "totalGross")]
        public Money TotalGross { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public TaxedItemPrice(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            this.TotalNet = new Money(data.totalNet);
            this.TotalGross = new Money(data.totalGross);
        }

        #endregion
    }
}
