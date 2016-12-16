using System;

using commercetools.Common;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace commercetools.Payments
{
    /// <summary>
    /// TransactionDraft
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-payments.html#transactiondraft"/>
    public class TransactionDraft
    {
        #region Properties

        [JsonProperty(PropertyName = "timestamp")]
        public DateTime? Timestamp { get; set;}

        [JsonProperty(PropertyName = "type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TransactionType? Type { get; set;}

        [JsonProperty(PropertyName = "amount")]
        public Money Amount { get; set;}

        [JsonProperty(PropertyName = "interactionId")]
        public string InteractionId { get; set;}

        [JsonProperty(PropertyName = "state")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TransactionState? State { get; set;}

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public TransactionDraft(TransactionType type, Money amount)
        {
            this.Type = type;
            this.Amount = amount;
        }

        #endregion
    }
}
