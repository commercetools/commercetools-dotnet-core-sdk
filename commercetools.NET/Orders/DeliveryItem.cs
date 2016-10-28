using System;

using Newtonsoft.Json;

namespace commercetools.Orders
{
    /// <summary>
    /// DeliveryItem
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#deliveryitem"/>
    public class DeliveryItem
    {
        #region Properties

        [JsonProperty(PropertyName = "id")]
        public string Id { get; private set; }

        [JsonProperty(PropertyName = "quantity")]
        public int? Quantity { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public DeliveryItem(dynamic data = null)
        {
            if (data == null)
            {
                return;
            }

            this.Id = data.id;
            this.Quantity = data.quantity;
        }

        #endregion
    }
}
