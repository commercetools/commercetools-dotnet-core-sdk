using commercetools.Common;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace commercetools.Orders.UpdateActions
{
    /// <summary>
    /// In order to set a ReturnShipmentState, there needs to be at least one ReturnInfo with at least one ReturnItem created before.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#set-returnshipmentstate"/>
    public class SetReturnShipmentStateAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// ID of the concerning ReturnItem
        /// </summary>
        [JsonProperty(PropertyName = "returnItemId")]
        public string ReturnItemId { get; set; }

        /// <summary>
        /// Shipment state
        /// </summary>
        [JsonProperty(PropertyName = "shipmentState")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ReturnShipmentState ShipmentState { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="returnItemId">ID of the concerning ReturnItem</param>
        /// <param name="shipmentState">Shipment state</param>
        public SetReturnShipmentStateAction(string returnItemId, ReturnShipmentState shipmentState)
        {
            this.Action = "setReturnShipmentState";
            this.ReturnItemId = returnItemId;
            this.ShipmentState = shipmentState;
        }

        #endregion
    }
}
