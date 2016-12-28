using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace commercetools.Orders
{
    /// <summary>
    /// The ReturnItemDraft needs to be given with the Add ReturnInfo update method.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#returnitemdraft"/>
    public class ReturnItemDraft
    {
        #region Properties

        [JsonProperty(PropertyName = "quantity")]
        public int Quantity { get; set; }

        [JsonProperty(PropertyName = "lineItemId")]
        public string LineItemId { get; set; }

        [JsonProperty(PropertyName = "comment")]
        public string Comment { get; set; }

        [JsonProperty(PropertyName = "shipmentState")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ReturnShipmentState ShipmentState { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="quantity">Quantity</param>
        /// <param name="lineItemId">Line item ID</param>
        /// <param name="shipmentState">ReturnShipmentState</param>
        public ReturnItemDraft(int quantity, string lineItemId, ReturnShipmentState shipmentState)
        {
            this.Quantity = quantity;
            this.LineItemId = lineItemId;
            this.ShipmentState = shipmentState;
        }

        #endregion
    }
}
