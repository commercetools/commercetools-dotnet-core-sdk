using System;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Messages
{
    /// <summary>
    /// This message is the result of the transitionCustomLineItemState update action.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-messages.html#customlineitemstatetransition-message"/>
    public class CustomLineItemStateTransitionMessage : Message
    {
        #region Properties

        [JsonProperty(PropertyName = "customLineItemId")]
        public string CustomLineItemId { get; private set; }

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
        public CustomLineItemStateTransitionMessage(dynamic data) 
            : base((object)data)
        {
            if (data == null)
            {
                return;
            }

            this.CustomLineItemId = data.customLineItemId;
            this.TransitionDate = data.transitionDate;
            this.Quantity = data.quantity;
            this.FromState = new Reference(data.fromState);
            this.ToState = new Reference(data.toState);
        }

        #endregion
    }
}
