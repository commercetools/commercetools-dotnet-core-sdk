using commercetools.Payments;

using Newtonsoft.Json;

namespace commercetools.Messages
{
    /// <summary>
    /// This message is the result of the addTransaction update action.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-messages.html#paymenttransactionadded-message"/>
    public class PaymentTransactionAddedMessage : Message
    {
        #region Properties

        /// <summary>
        /// Transaction
        /// </summary>
        [JsonProperty(PropertyName = "transaction")]
        public Transaction Transaction { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public PaymentTransactionAddedMessage(dynamic data)
            : base((object)data)
        {
            if (data == null)
            {
                return;
            }

            this.Transaction = new Transaction(data.transaction);
        }

        #endregion
    }
}
