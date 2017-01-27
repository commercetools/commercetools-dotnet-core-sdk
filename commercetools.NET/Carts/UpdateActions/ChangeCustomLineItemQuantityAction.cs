using System;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Carts.UpdateActions
{
    /// <summary>
    /// Sets the quantity of the given CustomLineItem.
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-carts.html#change-customlineitem-quantity"/>
    public class ChangeCustomLineItemQuantityAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Id of an existing LineItem in the cart.
        /// </summary>
        [JsonProperty(PropertyName = "customLineItemId")]
        public string CustomLineItemId { get; set; }

        /// <summary>
        /// The new quantity being a value > 0.
        /// </summary>
        /// <remarks>
        /// In case the item should be removed from the cart at all, use remove CustomLineItem instead.
        /// </remarks>
        [JsonProperty(PropertyName = "quantity")]
        public int Quantity { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="customLineItemId">Id of an existing LineItem in the cart</param>
        /// <param name="quantity">The new quantity being a value > 0.</param>
        public ChangeCustomLineItemQuantityAction(string customLineItemId, int quantity)
        {
            if (quantity < 1)
            {
                throw new ArgumentException("quantity must have a value greater than zero.");
            }

            this.Action = "changeLineItemQuantity";
            this.CustomLineItemId = customLineItemId;
            this.Quantity = quantity;
        }

        #endregion
    }
}
