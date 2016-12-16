using System;

using commercetools.Common;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace commercetools.Payments
{
    /// <summary>
    /// A representation of a financial transactions.
    /// </summary>
    /// <remarks>
    /// Transactions are either created by the solution implementation to trigger a new transaction at the PSP or created by the PSP integration as the result of a notification by the PSP.
    /// </remarks>
    /// <see href="http://dev.commercetools.com/http-api-projects-payments.html#transaction"/>
    public class Transaction
    {
        #region Properties

        [JsonProperty(PropertyName = "id")]
        public string Id { get; private set; }

        [JsonProperty(PropertyName = "timestamp")]
        public DateTime? Timestamp { get; private set; }

        [JsonProperty(PropertyName = "type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TransactionType? Type { get; private set; }

        [JsonProperty(PropertyName = "amount")]
        public Money Amount { get; private set; }

        [JsonProperty(PropertyName = "interactionId")]
        public string InteractionId { get; private set; }

        [JsonProperty(PropertyName = "state")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TransactionState? State { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public Transaction()
        {
        }

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public Transaction(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            TransactionType type;
            TransactionState state;

            string typeStr = (data.type != null ? data.type.ToString() : string.Empty);
            string stateStr = (data.state != null ? data.state.ToString() : string.Empty);

            this.Id = data.id;
            this.Type = Enum.TryParse(typeStr, out type) ? (TransactionType?)type : null;
            this.Timestamp = data.timestamp;
            this.Amount = new Money(data.amount);
            this.InteractionId = data.interactionId;
            this.State = Enum.TryParse(stateStr, out state) ? (TransactionState?)state : null;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            Transaction transaction = obj as Transaction;

            if (transaction == null)
            {
                return false;
            }

            return transaction.Id.Equals(this.Id);
        }

        /// <summary>
        /// GetHashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        #endregion
    }
}
