using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Carts.UpdateActions
{
    /// <summary>
    /// Sets the quantity of the given LineItem. 
    /// </summary>
    /// <remarks>
    /// If quantity is 0, line item is removed from the cart.
    /// </remarks>
    /// <see href="https://dev.commercetools.com/http-api-projects-carts.html#change-lineitem-quantity"/>
    public class ChangeLineItemQuantityAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Id of an existing LineItem in the cart.
        /// </summary>
        [JsonProperty(PropertyName = "lineItemId")]
        public string LineItemId { get; set; }

        /// <summary>
        /// Number
        /// </summary>
        [JsonProperty(PropertyName = "quantity")]
        public int Quantity { get; set; }
 
        /// <summary>
        /// Sets the line item price to the given value and sets the line item priceMode to ExternalPrice LineItemPriceMode.
        /// </summary>
        [JsonProperty(PropertyName = "externalPrice")]
        public Money ExternalPrice { get; set; }

        /// <summary>
        /// Sets the line item price and totalPrice to the given values and sets the line item priceMode to ExternalTotal LineItemPriceMode.
        /// </summary>
        [JsonProperty(PropertyName = "externalTotalPrice")]
        public ExternalLineItemTotalPrice ExternalTotalPrice { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="lineItemId">Id of an existing LineItem in the cart</param>
        /// <param name="quantity">Number</param>
        public ChangeLineItemQuantityAction(string lineItemId, int quantity)
        {
            this.Action = "changeLineItemQuantity";
            this.LineItemId = lineItemId;
            this.Quantity = quantity;
        }

        #endregion
    }
}
