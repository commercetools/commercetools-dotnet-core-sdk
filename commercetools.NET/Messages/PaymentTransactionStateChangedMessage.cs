using commercetools.Common;
using commercetools.Payments;

using Newtonsoft.Json;

namespace commercetools.Messages
{
    /// <summary>
    /// This message is the result of the changeTransactionState update action.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-messages.html#paymenttransactionstatechanged-message"/>
    public class PaymentTransactionStateChangedMessage : Message
    {
        #region Properties

        /// <summary>
        /// Transaction ID
        /// </summary>
        [JsonProperty(PropertyName = "transactionId")]
        public string TransactionId { get; private set; }

        /// <summary>
        /// State
        /// </summary>
        [JsonProperty(PropertyName = "state")]
        public TransactionState? State { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public PaymentTransactionStateChangedMessage(dynamic data)
            : base((object)data)
        {
            if (data == null)
            {
                return;
            }

            TransactionState? state;

            this.TransactionId = data.transactionId;
            this.State = Helper.TryGetEnumByEnumMemberAttribute<TransactionState?>((string)data.state, out state) ? state : null;
        }

        #endregion
    }
}
