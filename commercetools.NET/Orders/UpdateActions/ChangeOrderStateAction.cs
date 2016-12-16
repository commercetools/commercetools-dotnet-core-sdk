using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Orders.UpdateActions
{
    /// <summary>
    /// ChangeOrderStateAction
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#change-orderstate"/>
    public class ChangeOrderStateAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// OrderState
        /// </summary>
        [JsonProperty(PropertyName = "orderState")]
        public OrderState OrderState { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="orderState">OrderState</param>
        public ChangeOrderStateAction(OrderState orderState)
        {
            this.Action = "changeOrderState";
            this.OrderState = orderState;
        }

        #endregion
    }
}
