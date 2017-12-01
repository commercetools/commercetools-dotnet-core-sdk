using System.Collections.Generic;

using Newtonsoft.Json;

namespace commercetools.Subscriptions
{
    /// <summary>
    /// SubscriptionDraft
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-subscriptions.html#subscriptiondraft"/>
    public class SubscriptionDraft
    {
        #region Properties

        /// <summary>
        /// User-specific unique identifier for the subscription
        /// </summary>
        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }

        /// <summary>
        /// The Message Queue into which the notifications are to be sent
        /// </summary>
        [JsonProperty(PropertyName = "destination")]
        public Destination Destination { get; set; }

        /// <summary>
        /// The messages to be subscribed to.
        /// </summary>
        [JsonProperty(PropertyName = "messages")]
        public List<MessageSubscription> Messages { get; private set; }

        /// <summary>
        /// The change notifications to be subscribed to.
        /// </summary>
        [JsonProperty(PropertyName = "changes")]
        public List<ChangeSubscription> Changes { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <remarks>
        /// Either messages or changes need to be set.
        /// </remarks>
        /// <param name="destination">The Message Queue into which the notifications are to be sent</param>
        /// <param name="messages">The messages to be subscribed to.</param>
        /// <param name="changes">The change notifications to be subscribed to.</param>
        public SubscriptionDraft(Destination destination, List<MessageSubscription> messages = null, List<ChangeSubscription> changes = null)
        {
            this.Destination = destination;
            this.Messages = messages;
            this.Changes = changes;
        }

        #endregion
    }
}
