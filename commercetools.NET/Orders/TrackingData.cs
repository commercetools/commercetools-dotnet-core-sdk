using Newtonsoft.Json;

namespace commercetools.Orders
{
    /// <summary>
    /// Tracking data is usually some info about the delivery (like a DHL tracking number) which is useful to keep an eye on your delivery, view its status, etc.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#trackingdata"/>
    public class TrackingData
    {
        #region Properties

        [JsonProperty(PropertyName = "trackingId")]
        public string TrackingId { get; private set; }

        [JsonProperty(PropertyName = "carrier")]
        public string Carrier { get; private set; }

        [JsonProperty(PropertyName = "provider")]
        public string Provider { get; private set; }

        [JsonProperty(PropertyName = "providerTransaction")]
        public string ProviderTransaction { get; private set; }

        [JsonProperty(PropertyName = "isReturn")]
        public bool? IsReturn { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public TrackingData(dynamic data = null)
        {
            if (data == null)
            {
                return;
            }

            this.TrackingId = data.trackingId;
            this.Carrier = data.carrier;
            this.Provider = data.provider;
            this.ProviderTransaction = data.providerTransaction;
            this.IsReturn = data.isReturn;
        }

        #endregion
    }
}
