using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Payments.UpdateActions
{
    /// <summary>
    /// Changes state of a transaction.
    /// </summary>
    /// <remarks>
    /// If a transaction has been executed asynchronously, it's state can be updated. E.g. if a Refund was executed, the state can be set to Success.
    /// </remarks>
    /// <see href="https://dev.commercetools.com/http-api-projects-payments.html#change-transactionstate"/>
    public class ChangeTransactionStateAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// UUID of the transaction to be updated
        /// </summary>
        [JsonProperty(PropertyName = "transactionId")]
        public string TransactionId { get; set; }

        /// <summary>
        /// The new state of this transaction.
        /// </summary>
        [JsonProperty(PropertyName = "state")]
        public TransactionState State { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="transactionId">UUID of the transaction to be updated</param>
        /// <param name="state">The new state of this transaction.</param>
        public ChangeTransactionStateAction(string transactionId, TransactionState state)
        {
            this.Action = "changeTransactionState";
            this.TransactionId = transactionId;
            this.State = state;
        }

        #endregion
    }
}
