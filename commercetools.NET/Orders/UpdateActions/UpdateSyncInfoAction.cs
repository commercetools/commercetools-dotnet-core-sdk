using System;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Orders.UpdateActions
{
    /// <summary>
    /// UpdateSyncInfoAction
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#update-syncinfo"/>
    public class UpdateSyncInfoAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Reference to a channel.
        /// </summary>
        [JsonProperty(PropertyName = "channel")]
        public Reference Channel { get; set; }

        /// <summary>
        /// Can be used to reference an external order instance, file etc.
        /// </summary>
        [JsonProperty(PropertyName = "externalId")]
        public string ExternalId { get; set; }

        /// <summary>
        /// If not provided, then current date would be used
        /// </summary>
        [JsonProperty(PropertyName = "syncedAt")]
        public DateTime SyncedAt { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public UpdateSyncInfoAction()
        {
            this.Action = "updateSyncInfo";
        }

        #endregion
    }
}
