using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Payments.UpdateActions
{
    /// <summary>
    /// Changes how much money this payment intends to receive from the customer.
    /// </summary>
    /// <remarks>
    /// The value usually matches the cart or order gross total. Updating the amountPlanned may be required after a customer changed the cart or added/removed a CartDiscount during the checkout.
    /// </remarks>
    /// <see href="http://dev.commercetools.com/http-api-projects-payments.html#change-amountplanned"/>
    public class ChangeAmountPlannedAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// The new amountPlanned
        /// </summary>
        [JsonProperty(PropertyName = "amount")]
        public Money Amount { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="amount">The new amountPlanned</param>
        public ChangeAmountPlannedAction(Money amount)
        {
            this.Action = "changeAmountPlanned";
            this.Amount = amount;
        }

        #endregion
    }
}
