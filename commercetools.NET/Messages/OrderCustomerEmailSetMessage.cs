using commercetools.Orders;

using Newtonsoft.Json;

namespace commercetools.Messages
{
    /// <summary>
    /// This message is created when the customer email of an existing order is modified. 
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-messages.html#ordercustomeremailset-message"/>
    public class OrderCustomerEmailSetMessage : Message
    {
        #region Properties

        /// <summary>
        /// Email
        /// </summary>
        [JsonProperty(PropertyName = "email")]
        public string Email { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public OrderCustomerEmailSetMessage(dynamic data = null)
            : base((object)data)
        {
            if (data == null)
            {
                return;
            }

            this.Email = data.email;
        }

        #endregion
    }
}
