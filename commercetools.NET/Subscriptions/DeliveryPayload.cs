using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Subscriptions
{
    /// <summary>
    /// DeliveryPayload
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-subscriptions.html#delivery-payloads"/>
    public class DeliveryPayload
    {
        #region Properties

        /// <summary>
        /// The key of the project. Useful if the same queue is filled from multiple projects.
        /// </summary>
        [JsonProperty(PropertyName = "projectKey")]
        public string ProjectKey { get; private set; }

        /// <summary>
        /// Identifies the payload
        /// </summary>
        [JsonProperty(PropertyName = "notificationType")]
        public string NotificationType { get; private set; }

        /// <summary>
        /// A reference to the resource that triggered this delivery.
        /// </summary>
        [JsonProperty(PropertyName = "resource")]
        public Reference Resource { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        protected DeliveryPayload(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            this.ProjectKey = data.projectKey;
            this.NotificationType = data.notificationType;
            this.Resource = new Reference(data.resource);
        }

        #endregion
    }
}
