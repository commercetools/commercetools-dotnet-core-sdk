using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.ShippingMethods.UpdateActions
{
    /// <summary>
    /// Remove ShippingRate
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-shippingMethods.html#remove-shippingrate"/>
    public class RemoveShippingRateAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Reference to a Zone
        /// </summary>
        [JsonProperty(PropertyName = "zone")]
        public Reference Zone { get; set; }

        /// <summary>
        /// Shipping Rate
        /// </summary>
        [JsonProperty(PropertyName = "shippingRate")]
        public ShippingRate ShippingRate { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="zone">Reference to a Zone</param>
        /// <param name="shippingRate">Shipping Rate</param>
        public RemoveShippingRateAction(Reference zone, ShippingRate shippingRate)
        {
            this.Action = "removeShippingRate";
            this.Zone = zone;
            this.ShippingRate = shippingRate;
        }

        #endregion
    }
}
