using System.Collections.Generic;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.ShippingMethods
{
    /// <summary>
    /// Defines shipping rates (prices) for a specific zone. Shipping rates is an array because the rates for different currencies can be defined.
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-shippingMethods.html#zonerate"/>
    public class ZoneRate
    {
        #region Properties

        [JsonProperty(PropertyName = "zone")]
        public Reference Zone { get; set; }

        [JsonProperty(PropertyName = "shippingRates")]
        public List<ShippingRate> ShippingRates { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public ZoneRate()
        {
        }

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public ZoneRate(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            this.Zone = data.zone != null ? new Reference(data.zone) : null;
            this.ShippingRates = Helper.GetListFromJsonArray<ShippingRate>(data.shippingRates);
        }

        #endregion
    }
}
