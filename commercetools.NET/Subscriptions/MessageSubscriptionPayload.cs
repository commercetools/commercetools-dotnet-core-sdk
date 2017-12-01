namespace commercetools.Subscriptions
{
    /// <summary>
    /// This payload will be sent for a MessageSubscription.
    /// </summary>
    /// <remarks>
    /// The payload will always contain the common fields id, version, sequenceNumber, resourceVersion, createdAt and lastModifiedAt for any message. If the payload fits within the size limit of your message queue (the limit is often 256kb), all additional fields for the specific message are included as well (along with the type field). If the payload did not fit, it can be retrieved from the Messages endpoint if messages are enabled.
    /// </remarks>
    /// <see href="https://dev.commercetools.com/http-api-projects-subscriptions.html#message-subscription-payload"/>
    public class MessageSubscriptionPayload : DeliveryPayload
    {
        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public MessageSubscriptionPayload(dynamic data)
            : base((object)data)
        {
        }

        #endregion
    }
}
