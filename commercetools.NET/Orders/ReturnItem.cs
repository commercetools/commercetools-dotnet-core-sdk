using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace commercetools.Orders
{
    /// <summary>
    /// ReturnItem
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#returnitem"/>
    public class ReturnItem
    {
        #region Properties

        [JsonProperty(PropertyName = "id")]
        public string Id { get; private set; }

        [JsonProperty(PropertyName = "quantity")]
        public int? Quantity { get; private set; }

        [JsonProperty(PropertyName = "lineItemId")]
        public string LineItemId { get; private set; }

        [JsonProperty(PropertyName = "comment")]
        public string Comment { get; private set; }

        [JsonProperty(PropertyName = "shipmentState")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ReturnShipmentState? ShipmentState { get; private set; }

        [JsonProperty(PropertyName = "paymentState")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ReturnPaymentState? PaymentState { get; private set; }

        [JsonProperty(PropertyName = "lastModifiedAt")]
        public DateTime? LastModifiedAt { get; private set; }

        [JsonProperty(PropertyName = "createdAt")]
        public DateTime? CreatedAt { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public ReturnItem(dynamic data = null)
        {
            if (data == null)
            {
                return;
            }

            ReturnShipmentState shipmentState;
            ReturnPaymentState paymentState;

            string shipmentStateStr = (data.shipmentState != null ? data.shipmentState.ToString() : string.Empty);
            string paymentStateStr = (data.paymentState != null ? data.paymentState.ToString() : string.Empty);

            this.Id = data.id;
            this.Quantity = data.quantity;
            this.LineItemId = data.lineItemId;
            this.Comment = data.comment;
            this.ShipmentState = Enum.TryParse(shipmentStateStr, out shipmentState) ? (ReturnShipmentState?)shipmentState : null;
            this.PaymentState = Enum.TryParse(paymentStateStr, out paymentState) ? (ReturnPaymentState?)paymentState : null;
            this.LastModifiedAt = data.lastModifiedAt;
            this.CreatedAt = data.createdAt;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            ReturnItem returnItem = obj as ReturnItem;

            if (returnItem == null)
            {
                return false;
            }

            return (returnItem.Id.Equals(this.Id));
        }

        /// <summary>
        /// GetHashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        #endregion
    }
}
