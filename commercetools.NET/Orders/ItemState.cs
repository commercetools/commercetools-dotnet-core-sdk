using System;

using commercetools.States;

using Newtonsoft.Json;

namespace commercetools.Orders
{
    /// <summary>
    /// For item states we also need the information how much of the quantity is affected by this state.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#itemstate"/>
    public class ItemState
    {
        #region Properties

        [JsonProperty(PropertyName = "quantity")]
        public int? Quantity { get; private set; }

        [JsonProperty(PropertyName = "state")]
        public State State { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public ItemState(dynamic data = null)
        {
            if (data == null)
            {
                return;
            }

            this.Quantity = data.quantity;
            this.State = new State(data.state);
        }

        #endregion
    }
}
