using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Payments.UpdateActions
{
    /// <summary>
    /// Sets the amount of money that has been refunded to the customer.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-payments.html#set-amountrefunded"/>
    public class SetAmountRefundedAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// If not provided, the amount is unset.
        /// </summary>
        [JsonProperty(PropertyName = "amount")]
        public Money Amount { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public SetAmountRefundedAction()
        {
            this.Action = "setAmountRefunded";
        }

        #endregion
    }
}
