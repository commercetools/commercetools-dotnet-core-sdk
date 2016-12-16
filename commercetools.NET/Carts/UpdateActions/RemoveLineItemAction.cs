using System;
using System.Collections.Generic;

using commercetools.Common;
using commercetools.CustomFields;

using Newtonsoft.Json;

namespace commercetools.Carts.UpdateActions
{
    /// <summary>
    /// Decreases the quantity of the given LineItem.
    /// </summary>
    /// <remarks>
    /// If after the update the quantity of the line item is not greater than 0 or the quantity is not specified, the line item is removed from the cart.
    /// </remarks>
    /// <see href="https://dev.commercetools.com/http-api-projects-carts.html#remove-lineitem"/>
    public class RemoveLineItemAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Id of an existing LineItem in the cart.
        /// </summary>
        [JsonProperty(PropertyName = "lineItemId")]
        public string LineItemId  { get; set; }

        /// <summary>
        /// Number
        /// </summary>
        [JsonProperty(PropertyName = "quantity")]
        public int Quantity { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="lineItemId">Id of an existing LineItem in the cart</param>
        /// <param name="quantity">Number</param>
        public RemoveLineItemAction(string lineItemId, int quantity = 0)
        {
            this.Action = "removeLineItem";
            this.LineItemId = lineItemId;
            this.Quantity = quantity;
        }

        #endregion
    }
}
