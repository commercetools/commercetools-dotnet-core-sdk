using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.ShippingMethods.UpdateActions
{
    /// <summary>
    /// Remove Zone
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-shippingMethods.html#remove-zone"/>
    public class RemoveZoneAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Reference to a Zone
        /// </summary>
        [JsonProperty(PropertyName = "zone")]
        public Reference Zone { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="zone">Reference to a Zone</param>
        public RemoveZoneAction(Reference zone)
        {
            this.Action = "removeZone";
            this.Zone = zone;
        }

        #endregion
    }
}
