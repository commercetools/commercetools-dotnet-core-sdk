using System;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Orders.UpdateActions
{
    /// <summary>
    /// TransitionLineItemStateAction
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#change-the-state-of-lineitem-according-to-allowed-transitions"/>
    public class TransitionLineItemStateAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Line item ID
        /// </summary>
        [JsonProperty(PropertyName = "lineItemId")]
        public string LineItemId { get; set; }

        /// <summary>
        /// Number
        /// </summary>
        [JsonProperty(PropertyName = "quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// Reference to a state
        /// </summary>
        [JsonProperty(PropertyName = "fromState")]
        public Reference FromState { get; set; }

        /// <summary>
        /// Reference to a state
        /// </summary>
        [JsonProperty(PropertyName = "toState")]
        public Reference ToState { get; set; }

        /// <summary>
        /// Actual transition date
        /// </summary>
        [JsonProperty(PropertyName = "actualTransitionDate")]
        public DateTime ActualTransitionDate { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="lineItemId">Line item ID</param>
        /// <param name="quantity">Quantity</param>
        /// <param name="fromState">Reference to a state</param>
        /// <param name="toState">Reference to a state</param>
        public TransitionLineItemStateAction(string lineItemId, int quantity, Reference fromState, Reference toState)
        {
            this.Action = "transitionLineItemState";
            this.LineItemId = lineItemId;
            this.Quantity = quantity;
            this.FromState = fromState;
            this.ToState = toState;
        }

        #endregion
    }
}
