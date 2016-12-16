using commercetools.Common;
using commercetools.CustomFields;

using Newtonsoft.Json;

namespace commercetools.Carts.UpdateActions
{
    /// <summary>
    /// This action adds a payment to the PaymentInfo.
    /// </summary>
    /// <remarks>
    /// The payment must not be assigned to another Order or active Cart yet.
    /// </remarks>
    /// <see href="https://dev.commercetools.com/http-api-projects-carts.html#add-payment"/>
    public class AddPaymentAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Reference to a Payment.
        /// </summary>
        [JsonProperty(PropertyName = "payment")]
        public Reference Payment { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="payment">Reference to a Payment.</param>
        public AddPaymentAction(Reference payment)
        {
            this.Action = "addPayment";
            this.Payment = payment;
        }

        #endregion
    }
}
