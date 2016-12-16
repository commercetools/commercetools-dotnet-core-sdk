using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Orders.UpdateActions
{
    /// <summary>
    /// Sets a string that uniquely identifies an order. It can be used to create more human-readable (in contrast to ID) identifier for the order.
    /// </summary>>
    /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#set-order-number"/>
    public class SetOrderNumberAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// It should be unique across a project. Once it's set, it cannot be changed.
        /// </summary>
        [JsonProperty(PropertyName = "orderNumber")]
        public string OrderNumber { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="orderNumber">It should be unique across a project. Once it's set, it cannot be changed.</param>
        public SetOrderNumberAction(string orderNumber)
        {
            this.Action = "setOrderNumber";
            this.OrderNumber = orderNumber;
        }

        #endregion
    }
}
