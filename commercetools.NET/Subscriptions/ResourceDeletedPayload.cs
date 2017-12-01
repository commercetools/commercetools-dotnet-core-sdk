using Newtonsoft.Json;

namespace commercetools.Subscriptions
{
    /// <summary>
    /// This payload will be sent for a ChangeSubscription if a resource was deleted.
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-subscriptions.html#resourcedeleted-payload"/>
    public class ResourceDeletedPayload : DeliveryPayload
    {
        #region Properties

        /// <summary>
        /// The version of the resource after it was created
        /// </summary>
        [JsonProperty(PropertyName = "version")]
        public int? Version { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public ResourceDeletedPayload(dynamic data)
            : base((object)data)
        {
            if (data == null)
            {
                return;
            }

            this.Version = data.version;
        }

        #endregion
    }
}
