using System;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Messages
{
    /// <summary>
    /// This message is the result of the transitionLineItemState update action.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-messages.html#lineitemstatetransition-message"/>
    public class LineItemStateTransitionMessage : Message
    {
        #region Properties

        [JsonProperty(PropertyName = "lineItemId")]
        public string LineItemId { get; private set; }

        [JsonProperty(PropertyName = "transitionDate")]
        public DateTime? TransitionDate { get; private set; }

        [JsonProperty(PropertyName = "quantity")]
        public int? Quantity { get; private set; }

        [JsonProperty(PropertyName = "fromState")]
        public Reference FromState { get; private set; }

        [JsonProperty(PropertyName = "toState")]
        public Reference ToState { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public LineItemStateTransitionMessage(dynamic data)
            : base((object)data)
        {
            if (data == null)
            {
                return;
            }

            this.LineItemId = data.lineItemId;
            this.TransitionDate = data.transitionDate;
            this.Quantity = data.quantity;
            this.FromState = new Reference(data.fromState);
            this.ToState = new Reference(data.toState);
        }

        #endregion
    }
}
