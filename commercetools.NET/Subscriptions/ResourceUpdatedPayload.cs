using Newtonsoft.Json;

namespace commercetools.Subscriptions
{
    /// <summary>
    /// This payload will be sent for a ChangeSubscription if a resource was updated.
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-subscriptions.html#resourceupdated-payload"/>
    public class ResourceUpdatedPayload : DeliveryPayload
    {
        #region Properties

        /// <summary>
        /// The version of the resource after the update
        /// </summary>
        [JsonProperty(PropertyName = "version")]
        public int Version { get; private set; }

        /// <summary>
        /// The version of the resource before the update
        /// </summary>
        [JsonProperty(PropertyName = "oldVersion")]
        public int OldVersion { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public ResourceUpdatedPayload(dynamic data)
            : base((object)data)
        {
            if (data == null)
            {
                return;
            }

            this.Version = data.version;
            this.OldVersion = data.oldVersion;
        }

        #endregion
    }
}
