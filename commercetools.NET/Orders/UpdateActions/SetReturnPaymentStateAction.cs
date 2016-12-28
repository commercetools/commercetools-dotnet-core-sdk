using commercetools.Common;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace commercetools.Orders.UpdateActions
{
    /// <summary>
    /// In order to set a ReturnPaymentState, there needs to be at least one ReturnInfo with at least one ReturnItem created before.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#set-returnpaymentstate"/>
    public class SetReturnPaymentStateAction : UpdateAction
    {
        #region Properties

        /// <summary>
        ///  ID of the concerning ReturnItem
        /// </summary>
        [JsonProperty(PropertyName = "returnItemId")]
        public string ReturnItemId { get; set; }

        /// <summary>
        /// Payment state
        /// </summary>
        [JsonProperty(PropertyName = "paymentState")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ReturnPaymentState PaymentState { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="returnItemId">ID of the concerning ReturnItem</param>
        /// <param name="paymentState">Payment state</param>
        public SetReturnPaymentStateAction(string returnItemId, ReturnPaymentState paymentState)
        {
            this.Action = "setReturnPaymentState";
            this.ReturnItemId = returnItemId;
            this.PaymentState = paymentState;
        }

        #endregion
    }
}
