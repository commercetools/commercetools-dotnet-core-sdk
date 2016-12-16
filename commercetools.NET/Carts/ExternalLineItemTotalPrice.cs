using Newtonsoft.Json;

using commercetools.Common;

namespace commercetools.Carts
{
    /// <summary>
    /// ExternalLineItemTotalPrice
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-carts.html#externallineitemtotalprice"/>
    public class ExternalLineItemTotalPrice
    {
        #region Properties

        [JsonProperty(PropertyName = "price")]
        public Money Price { get; set; }

        [JsonProperty(PropertyName = "totalPrice")]
        public Money TotalPrice { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public ExternalLineItemTotalPrice()
        {
        }

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public ExternalLineItemTotalPrice(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            this.Price = new Money(data.price);
            this.TotalPrice = new Money(data.totalPrice);
        }

        #endregion
    }
}
