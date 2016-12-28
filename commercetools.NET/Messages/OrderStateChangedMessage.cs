using commercetools.Common;
using commercetools.Orders;

using Newtonsoft.Json;

namespace commercetools.Messages
{
    /// <summary>
    /// This message is created when the OrderState is changed.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-messages.html#orderstatechanged-message"/>
    public class OrderStateChangedMessage : Message
    {
        #region Properties

        [JsonProperty(PropertyName = "orderState")]
        public OrderState? OrderState { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public OrderStateChangedMessage(dynamic data)
            : base((object)data)
        {
            if (data == null)
            {
                return;
            }

            OrderState? orderState;

            this.OrderState = Helper.TryGetEnumByEnumMemberAttribute<OrderState?>((string)data.orderState, out orderState) ? orderState : null;
        }

        #endregion
    }
}
