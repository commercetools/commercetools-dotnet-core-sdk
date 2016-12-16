using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Carts.UpdateActions
{
    /// <summary>
    /// Sets the totalPrice and price of the line item and changes the priceMode of the line item to ExternalTotal.
    /// </summary>
    /// <remarks>
    ///  If the external total price of the line item was already set and the externalTotalPrice field of this update action is not given, the external price is unset and the line item price mode is set back to Platform. Although both price and totalPrice are set with this update action, only totalPrice will be used to calculate the total price of the cart.
    /// </remarks>
    /// <see href="https://dev.commercetools.com/http-api-projects-carts.html#set-lineitem-totalprice"/>
    public class SetLineItemTotalPriceAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Id of the LineItem.
        /// </summary>
        [JsonProperty(PropertyName = "lineItemId")]
        public string LineItemId { get; set; }

        /// <summary>
        /// ExternalLineItemTotalPrice
        /// </summary>
        [JsonProperty(PropertyName = "externalTotalPrice")]
        public ExternalLineItemTotalPrice ExternalLineItemTotalPrice { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="lineItemId">Id of an existing Product.</param>
        public SetLineItemTotalPriceAction(string lineItemId)
        {
            this.Action = "setLineItemTotalPrice";
            this.LineItemId = lineItemId;
        }

        #endregion
    }
}
