using System.Collections.Generic;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Subscriptions
{
    /// <summary>
    /// MessageSubscription
    /// </summary>
    /// <remarks>
    /// For supported resources and message types, please refer to the Messages Documentation. Messages will be delivered even if the Messages REST API is disabled. The Message Subscription Payload is delivered.
    /// </remarks>
    /// <see href="https://dev.commercetools.com/http-api-projects-subscriptions.html#messagesubscription"/>
    public class MessageSubscription
    {
        #region Properties

        /// <summary>
        /// Resource Type ID
        /// </summary>
        [JsonProperty(PropertyName = "resourceTypeId")]
        public string ResourceTypeId { get; private set; }

        /// <summary>
        /// types must contain valid message types for this resource, e.g. for resource type product the message type ProductPublished is valid. If no types of messages are given, the subscription is valid for all messages of this resource.
        /// </summary>
        [JsonProperty(PropertyName = "types")]
        public List<string> Types { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="resourceTypeId">Resource Type ID</param>
        public MessageSubscription(string resourceTypeId)
        {
            this.ResourceTypeId = resourceTypeId;
        }

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public MessageSubscription(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            this.ResourceTypeId = data.resourceTypeId;
            this.Types = Helper.GetListFromJsonArray<string>(data.types);
        }

        #endregion
    }
}
