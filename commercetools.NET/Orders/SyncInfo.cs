using System;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Orders
{
    /// <summary>
    /// Stores information about order synchronization activities (like export or import).
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#sync-info"/>
    public class SyncInfo
    {
        #region Properties

        [JsonProperty(PropertyName = "channel")]
        public Reference Channel { get; private set; }

        [JsonProperty(PropertyName = "externalId")]
        public string ExternalId { get; private set; }

        [JsonProperty(PropertyName = "syncedAt")]
        public DateTime? SyncedAt { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public SyncInfo(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            this.Channel = data.channel;
            this.ExternalId = data.externalId;
            this.SyncedAt = data.syncedAt;
        }

        #endregion
    }
}
