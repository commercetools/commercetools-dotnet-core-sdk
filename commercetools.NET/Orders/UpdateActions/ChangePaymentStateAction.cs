using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Orders.UpdateActions
{
    /// <summary>
    /// Changes the payment state.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#change-paymentstate"/>
    public class ChangePaymentStateAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// PaymentState
        /// </summary>
        [JsonProperty(PropertyName = "paymentState")]
        public PaymentState PaymentState { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="paymentState">PaymentState</param>
        public ChangePaymentStateAction(PaymentState paymentState)
        {
            this.Action = "changePaymentState";
            this.PaymentState = paymentState;
        }

        #endregion
    }
}
