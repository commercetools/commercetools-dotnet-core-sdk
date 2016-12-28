using commercetools.Orders;

using Newtonsoft.Json;

namespace commercetools.Messages
{
    /// <summary>
    /// This message is created when an order is imported.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-messages.html#orderimported-message"/>
    public class OrderImportedMessage : Message
    {
        #region Properties

        /// <summary>
        /// Order
        /// </summary>
        [JsonProperty(PropertyName = "order")]
        public Order Order { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public OrderImportedMessage(dynamic data)
            : base((object)data)
        {
            if (data == null)
            {
                return;
            }

            this.Order = new Order(data.order);
        }

        #endregion
    }
}
