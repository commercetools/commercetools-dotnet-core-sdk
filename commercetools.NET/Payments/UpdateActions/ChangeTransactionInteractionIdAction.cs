using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Payments.UpdateActions
{
    /// <summary>
    /// Changes the interactionId of a transaction.
    /// </summary>
    /// <remarks>
    ///  It should correspond to an Id of an interface interaction.
    /// </remarks>
    /// <see href="https://dev.commercetools.com/http-api-projects-payments.html#change-transactioninteractionid"/>
    public class ChangeTransactionInteractionIdAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// UUID of the transaction to be updated
        /// </summary>
        [JsonProperty(PropertyName = "transactionId")]
        public string TransactionId { get; set; }

        /// <summary>
        /// The new interactionId.
        /// </summary>
        [JsonProperty(PropertyName = "interactionId")]
        public string InteractionId { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="transactionId">UUID of the transaction to be updated</param>
        /// <param name="interactionId">The new interactionId.</param>
        public ChangeTransactionInteractionIdAction(string transactionId, string interactionId)
        {
            this.Action = "changeTransactionInteractionId";
            this.TransactionId = transactionId;
            this.InteractionId = interactionId;
        }

        #endregion
    }
}
