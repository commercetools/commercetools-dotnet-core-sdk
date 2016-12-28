using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Orders.UpdateActions
{
    /// <summary>
    /// This action adds a payment to the PaymentInfo. The payment must not be assigned to another Order or active Cart yet.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#add-payment"/>
    public class AddPaymentAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Reference to a Payment
        /// </summary>
        [JsonProperty(PropertyName = "payment")]
        public Reference Payment { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="payment">Reference to a Payment</param>
        public AddPaymentAction(Reference payment)
        {
            this.Action = "addPayment";
            this.Payment = payment;
        }

        #endregion
    }
}
