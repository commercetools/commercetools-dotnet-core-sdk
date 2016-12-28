using commercetools.Common;
using commercetools.Customers;

using Newtonsoft.Json;

namespace commercetools.Messages
{
    /// <summary>
    /// This message is the result of the create customer request.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-messages.html#customercreated-message"/>
    public class CustomerCreatedMessage : Message
    {
        #region Properties

        [JsonProperty(PropertyName = "customer")]
        public Customer Customer { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public CustomerCreatedMessage(dynamic data) 
            : base((object)data)
        {
            if (data == null)
            {
                return;
            }

            this.Customer = new Customer(data.customer);
        }

        #endregion
    }
}
