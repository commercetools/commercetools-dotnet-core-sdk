using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Payments.UpdateActions
{
    /// <summary>
    /// Adds a new financial transaction.
    /// </summary>
    /// <remarks>
    /// It can be used for asynchronous communication, e.g. one process could add a transaction of type Refund in state Pending and a PSP integration could asynchronously take care of executing the refund.
    /// </remarks>
    /// <see href="https://dev.commercetools.com/http-api-projects-payments.html#add-transaction"/>
    public class AddTransactionAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// TransactionDraft
        /// </summary>
        [JsonProperty(PropertyName = "transaction")]
        public TransactionDraft Transaction { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="transaction">TransactionDraft</param>
        public AddTransactionAction(TransactionDraft transaction)
        {
            this.Action = "addTransaction";
            this.Transaction = transaction;
        }

        #endregion
    }
}
